using System;
using System.Collections.Generic;
using VContainer;

public class MatchResolutionEngine : IMatchResolutionEngine
{
    private IMatchFinderService matchFinder;
    private IBoardStateService boardStateService;
    private IGridFactory gridFactory;
    private ITileTypeGenerator tileTypeGenerator;

    [Inject]
    private void Construct(IMatchFinderService matchFinder,IBoardStateService boardStateService,
        IGridFactory gridFactory,ITileTypeGenerator tileTypeGenerator)
    {
        this.matchFinder = matchFinder ?? throw new ArgumentNullException(nameof(matchFinder));
        this.boardStateService = boardStateService ?? throw new ArgumentNullException(nameof(boardStateService));
        this.gridFactory = gridFactory ?? throw new ArgumentNullException(nameof(gridFactory));
        this.tileTypeGenerator = tileTypeGenerator ?? throw new ArgumentNullException(nameof(tileTypeGenerator));
    }

    public bool TryBuildNextStep(GridModel grid, out MatchResolutionStep step)
    {
        var snapshot = new GridSnapshot(grid);
        var matches = matchFinder.FindMatches(snapshot);

        if (matches.Count == 0)
        {
            step = null;
            return false;
        }

        step = new MatchResolutionStep();
        step.Matches.AddRange(matches);
        step.Plan.RemovedCells.AddRange(ExtractCells(matches));

        var simulatedGrid = BuildWorkingGrid(snapshot, step.Plan.RemovedCells);
        BuildGravityPlan(simulatedGrid, step.Plan);
        BuildSpawnPlan(simulatedGrid, step.Plan);
        return true;
    }

    public bool TryReshuffle(GridModel grid, out GridSnapshot reshuffledGrid)
    {
        if (boardStateService.HasPossibleMove(new GridSnapshot(grid)))
        {
            reshuffledGrid = null;
            return false;
        }

        var replacement = gridFactory.Create(grid.Width, grid.Height);
        CopyGrid(replacement, grid);
        reshuffledGrid = new GridSnapshot(grid);
        return true;
    }

    private Cell[,] BuildWorkingGrid(GridSnapshot snapshot, List<GridPosition> removedCells)
    {
        var working = new Cell[snapshot.Width, snapshot.Height];
        for (int x = 0; x < snapshot.Width; x++)
        {
            for (int y = 0; y < snapshot.Height; y++)
            {
                working[x, y] = snapshot.Get(x, y);
            }
        }

        foreach (var pos in removedCells)
        {
            if (!GridRules.IsValidPosition(pos, snapshot.Width, snapshot.Height))
                continue;

            working[pos.X, pos.Y] = null;
        }

        return working;
    }

    private void BuildGravityPlan(Cell[,] workingGrid, ResolutionStepPlan plan)
    {
        var width = workingGrid.GetLength(0);
        var height = workingGrid.GetLength(1);

        for (int x = 0; x < width; x++)
        {
            int writeY = height - 1;

            for (int y = height - 1; y >= 0; y--)
            {
                var cell = workingGrid[x, y];

                if (cell == null)
                    continue;

                if (y != writeY)
                {
                    plan.GravityMoves.Add(new TileMove
                    {
                        From = new GridPosition(x, y),
                        To = new GridPosition(x, writeY),
                        tileType = cell.Type
                    });
                }

                workingGrid[x, writeY] = cell;
                if (writeY != y)
                    workingGrid[x, y] = null;

                writeY--;
            }
        }
    }

    private void BuildSpawnPlan(Cell[,] workingGrid, ResolutionStepPlan plan)
    {
        var width = workingGrid.GetLength(0);
        var height = workingGrid.GetLength(1);

        for (int x = 0; x < width; x++)
        {
            int spawnIndex = 0;

            for (int y = 0; y < height; y++)
            {
                if (workingGrid[x, y] != null)
                    continue;

                var tileType = tileTypeGenerator.GetRandom();
                plan.SpawnMoves.Add(new TileMove
                {
                    From = new GridPosition(x, -1 - spawnIndex),
                    To = new GridPosition(x, y),
                    tileType = tileType
                });

                workingGrid[x, y] = new Cell(tileType);
                spawnIndex++;
            }
        }
    }

    private List<GridPosition> ExtractCells(List<MatchGroup> matches)
    {
        var unique = new HashSet<GridPosition>();

        foreach (var match in matches)
        {
            foreach (var cell in match.Cells)
                unique.Add(cell);
        }

        return new List<GridPosition>(unique);
    }

    private static void CopyGrid(GridModel source, GridModel target)
    {
        for (int x = 0; x < source.Width; x++)
        {
            for (int y = 0; y < source.Height; y++)
            {
                target.Set(x, y, source.Get(x, y));
            }
        }
    }
}
