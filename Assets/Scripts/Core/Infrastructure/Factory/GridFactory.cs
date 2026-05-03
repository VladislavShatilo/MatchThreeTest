using System;
using UnityEngine;
using VContainer;

public class GridFactory : IGridFactory
{
    private IBoardStateService boardStateService;
    private const int MaxGenerationAttempts = 256;

    [Inject]
    private void Construct(IBoardStateService boardStateService)
    {
        this.boardStateService = boardStateService ?? throw new ArgumentNullException(nameof(boardStateService));
    }

    public GridModel Create(int width, int height)
    {
        var tileTypes = (TileType[])Enum.GetValues(typeof(TileType));

        for (int attempt = 0; attempt < MaxGenerationAttempts; attempt++)
        {
            var grid = new GridModel(width, height);
            FillWithoutStartingMatches(grid, tileTypes);

            if (boardStateService.HasPossibleMove(new GridSnapshot(grid)))
                return grid;
        }

        throw new InvalidOperationException($"Unable to generate a playable grid after {MaxGenerationAttempts} attempts.");
    }

    private void FillWithoutStartingMatches(GridModel grid, TileType[] tileTypes)
    {
        for (int y = 0; y < grid.Height; y++)
        {
            for (int x = 0; x < grid.Width; x++)
            {
                var candidates = Shuffle(tileTypes);

                for (int i = 0; i < candidates.Length; i++)
                {
                    var candidate = candidates[i];
                    if (CreatesImmediateMatch(grid, x, y, candidate))
                        continue;

                    grid.Set(x, y, new Cell(candidate));
                    break;
                }
            }
        }
    }

    private static bool CreatesImmediateMatch(GridModel grid, int x, int y, TileType type)
    {
        if (x >= 2 &&
            grid.Get(x - 1, y)?.Type == type &&
            grid.Get(x - 2, y)?.Type == type)
        {
            return true;
        }

        if (y >= 2 &&
            grid.Get(x, y - 1)?.Type == type &&
            grid.Get(x, y - 2)?.Type == type)
        {
            return true;
        }

        return false;
    }

    private TileType[] Shuffle(TileType[] source)
    {
        var result = (TileType[])source.Clone();

        for (int i = result.Length - 1; i > 0; i--)
        {
            var swapIndex = UnityEngine.Random.Range(0, i + 1);
            (result[i], result[swapIndex]) = (result[swapIndex], result[i]);
        }

        return result;
    }
}
