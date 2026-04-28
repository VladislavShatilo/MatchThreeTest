using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using VContainer;

public class MatchResolutionService : IMatchResolutionService
{
    private bool isResolving;
    private IMatchFinderService matchFinder;
    private IGridService gridService;
    private IEventBus eventBus;
    private IEventAwaiter eventAwaiter;
    private ITileTypeGenerator tileTypeGenerator;
    private IEnergyGainSystem energyGainSystem;


    [Inject]
    private void Construct(IMatchFinderService matchFinder, IGridService gridService, IEventBus eventBus,
        ITileTypeGenerator tileTypeGenerator, IEventAwaiter eventAwaiter, IEnergyGainSystem energyGainSystem)
    {
        this.matchFinder = matchFinder ?? throw new ArgumentNullException(nameof(matchFinder));
        this.gridService = gridService ?? throw new ArgumentNullException(nameof(gridService));
        this.eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        this.eventAwaiter = eventAwaiter ?? throw new ArgumentNullException(nameof(eventAwaiter));
        this.tileTypeGenerator = tileTypeGenerator ?? throw new ArgumentNullException(nameof(tileTypeGenerator));
        this.energyGainSystem = energyGainSystem ?? throw new ArgumentNullException(nameof(energyGainSystem));
    }

    public async UniTask Resolve(GridModel grid, CancellationToken ct)
    {
        if (isResolving)
            return;

        isResolving = true;
        try
        {
            while (true)
            {
                var snapshot = new GridSnapshot(grid);

                var matches = matchFinder.FindMatches(snapshot);

                if (matches.Count == 0)
                    break;
                foreach (var match in matches)
                {
                    energyGainSystem.OnMatch(match.Cells.Count);
                }
                var plan = new ResolutionStepPlan
                {
                    RemovedCells = ExtractCells(matches)
                };
                var simulatedGrid = BuildWorkingGrid(snapshot, plan.RemovedCells);
                BuildGravityPlan(simulatedGrid, plan);
                BuildSpawnPlan(simulatedGrid, plan);

                var removeAnimationCompleted = eventAwaiter.WaitAsync<GridAnimationCompletedEvent>(
                    e => e.Phase == GridAnimationPhase.Remove, ct);
                eventBus.Publish(new ResolutionPlanCreatedEvent(plan));

                await removeAnimationCompleted;
                ApplyRemovedToModel(grid, plan);
                eventBus.Publish(new GridStepAppliedEvent(GridAnimationPhase.Remove));

                var gravityAnimationCompleted = eventAwaiter.WaitAsync<GridAnimationCompletedEvent>(
                    e => e.Phase == GridAnimationPhase.Gravity, ct);
                await gravityAnimationCompleted;
                ApplyGravityToModel(grid, plan);
                eventBus.Publish(new GridStepAppliedEvent(GridAnimationPhase.Gravity));

                var refillAnimationCompleted = eventAwaiter.WaitAsync<GridAnimationCompletedEvent>(
                    e => e.Phase == GridAnimationPhase.Refill, ct);
                await refillAnimationCompleted;
                ApplySpawnToModel(grid, plan);
                eventBus.Publish(new GridStepAppliedEvent(GridAnimationPhase.Refill));

                var stepCompleted = eventAwaiter.WaitAsync<GridAnimationCompletedEvent>(
                    e => e.Phase == GridAnimationPhase.StepCompleted, ct);
                await stepCompleted;

                await UniTask.Delay(40, cancellationToken: ct);
                await UniTask.Yield(PlayerLoopTiming.LastPostLateUpdate, ct);
            }
        }
        finally
        {
            isResolving = false;
        }
    }

    private Cell[,] BuildWorkingGrid(GridSnapshot snapshot, List<Vector2Int> removedCells)
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

            working[pos.x, pos.y] = null;
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
                        From = new Vector2Int(x, y),
                        To = new Vector2Int(x, writeY),
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
                if (workingGrid[x, y] == null)
                {
                    var tileType = tileTypeGenerator.GetRandom();
                    plan.SpawnMoves.Add(new TileMove
                    {
                        From = new Vector2Int(x, -1 - spawnIndex),
                        To = new Vector2Int(x, y),
                        tileType = tileType
                    });

                    workingGrid[x, y] = new Cell(tileType);
                    spawnIndex++;
                }
            }
        }
    }

    private List<Vector2Int> ExtractCells(List<MatchGroup> matches)
    {
        var unique = new HashSet<Vector2Int>();

        foreach (var match in matches)
        {
            foreach (var cell in match.Cells)
                unique.Add(cell);
        }

        return new List<Vector2Int>(unique);
    }

    private void ApplyRemovedToModel(GridModel grid, ResolutionStepPlan plan)
    {
        foreach (var pos in plan.RemovedCells)
        {
            if (!GridRules.IsValidPosition(pos, grid.Width, grid.Height))
                continue;

            grid.Set(pos.x, pos.y, null);
        }
    }

    private void ApplyGravityToModel(GridModel grid, ResolutionStepPlan plan)
    {
        foreach (var move in plan.GravityMoves)
        {
            if (!GridRules.IsValidPosition(move.From, grid.Width, grid.Height) ||
                !GridRules.IsValidPosition(move.To, grid.Width, grid.Height))
            {
                continue;
            }

            var cell = grid.Get(move.From.x, move.From.y);
            if (cell == null)
                continue;

            grid.Set(move.From.x, move.From.y, null);
            grid.Set(move.To.x, move.To.y, cell);
        }
    }

    private void ApplySpawnToModel(GridModel grid, ResolutionStepPlan plan)
    {
        var occupiedTargets = new HashSet<Vector2Int>();
        foreach (var spawn in plan.SpawnMoves)
        {
            if (!GridRules.IsValidPosition(spawn.To, grid.Width, grid.Height))
                continue;
            if (!occupiedTargets.Add(spawn.To))
                continue;

            var cell = new Cell(spawn.tileType);
            grid.Set(spawn.To.x, spawn.To.y, cell);
        }
    }
}