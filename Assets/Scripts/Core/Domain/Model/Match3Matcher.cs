using System;
using System.Collections.Generic;

public class Match3Matcher
{
    private static readonly GridPosition[] NeighborOffsets =
    {
        GridPosition.Left,
        GridPosition.Right,
        GridPosition.Up,
        GridPosition.Down
    };

    public IReadOnlyList<MatchGroup> FindMatches(Match3Grid grid, int minimumRunLength = 3)
    {
        if (grid == null)
            throw new ArgumentNullException(nameof(grid));

        if (minimumRunLength < 3)
            throw new ArgumentOutOfRangeException(nameof(minimumRunLength));

        var matched = new bool[grid.Width, grid.Height];
        MarkHorizontalMatches(grid, matched, minimumRunLength);
        MarkVerticalMatches(grid, matched, minimumRunLength);
        return BuildMatchGroups(grid, matched);
    }

    private static void MarkHorizontalMatches(Match3Grid grid, bool[,] matched, int minimumRunLength)
    {
        for (int y = 0; y < grid.Height; y++)
        {
            int x = 0;
            while (x < grid.Width)
            {
                var tileType = grid.Get(x, y);
                if (!tileType.HasValue)
                {
                    x++;
                    continue;
                }

                var runEnd = x + 1;
                while (runEnd < grid.Width && grid.Get(runEnd, y) == tileType)
                    runEnd++;

                if (runEnd - x >= minimumRunLength)
                {
                    for (int i = x; i < runEnd; i++)
                        matched[i, y] = true;
                }

                x = runEnd;
            }
        }
    }

    private static void MarkVerticalMatches(Match3Grid grid, bool[,] matched, int minimumRunLength)
    {
        for (int x = 0; x < grid.Width; x++)
        {
            int y = 0;
            while (y < grid.Height)
            {
                var tileType = grid.Get(x, y);
                if (!tileType.HasValue)
                {
                    y++;
                    continue;
                }

                var runEnd = y + 1;
                while (runEnd < grid.Height && grid.Get(x, runEnd) == tileType)
                    runEnd++;

                if (runEnd - y >= minimumRunLength)
                {
                    for (int i = y; i < runEnd; i++)
                        matched[x, i] = true;
                }

                y = runEnd;
            }
        }
    }

    private static IReadOnlyList<MatchGroup> BuildMatchGroups(Match3Grid grid, bool[,] matched)
    {
        var visited = new bool[grid.Width, grid.Height];
        var groups = new List<MatchGroup>();
        var pending = new Queue<GridPosition>();

        for (int y = 0; y < grid.Height; y++)
        {
            for (int x = 0; x < grid.Width; x++)
            {
                if (!matched[x, y] || visited[x, y])
                    continue;

                var tileType = grid.Get(x, y);
                if (!tileType.HasValue)
                    continue;

                var cells = new List<GridPosition>();
                pending.Enqueue(new GridPosition(x, y));
                visited[x, y] = true;

                while (pending.Count > 0)
                {
                    var current = pending.Dequeue();
                    cells.Add(current);

                    for (int i = 0; i < NeighborOffsets.Length; i++)
                    {
                        var next = current + NeighborOffsets[i];
                        if (!IsInside(grid, next))
                            continue;

                        if (visited[next.X, next.Y] || !matched[next.X, next.Y])
                            continue;

                        if (grid.Get(next.X, next.Y) != tileType)
                            continue;

                        visited[next.X, next.Y] = true;
                        pending.Enqueue(next);
                    }
                }

                groups.Add(new MatchGroup(cells));
            }
        }

        return groups;
    }

    private static bool IsInside(Match3Grid grid, GridPosition position)
        => position.X >= 0 && position.X < grid.Width && position.Y >= 0 && position.Y < grid.Height;
}
