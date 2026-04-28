using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MatchFinderService : IMatchFinderService
{
    public List<MatchGroup> FindMatches(GridSnapshot grid)
    {
        var matched = new bool[grid.Width, grid.Height];
        MarkHorizontalMatches(grid, matched);
        MarkVerticalMatches(grid, matched);

        var cells = new List<Vector2Int>();
        for (int y = 0; y < grid.Height; y++)
        {
            for (int x = 0; x < grid.Width; x++)
            {
                if (matched[x, y])
                    cells.Add(new Vector2Int(x, y));
            }
        }

        if (cells.Count == 0)
            return new List<MatchGroup>();

        var ordered = cells
            .OrderBy(c => c.y)
            .ThenBy(c => c.x)
            .ToList();
        return new List<MatchGroup> { new MatchGroup(ordered) };
    }

    private void MarkHorizontalMatches(GridSnapshot grid, bool[,] matched)
    {
        for (int y = 0; y < grid.Height; y++)
        {
            int x = 0;
            while (x < grid.Width)
            {
                var startCell = grid.Get(x, y);
                if (startCell == null)
                {
                    x++;
                    continue;
                }

                int runEnd = x + 1;
                while (runEnd < grid.Width)
                {
                    var candidate = grid.Get(runEnd, y);
                    if (candidate == null || candidate.Type != startCell.Type)
                        break;

                    runEnd++;
                }

                int runLength = runEnd - x;
                if (runLength >= 3)
                {
                    for (int i = x; i < runEnd; i++)
                        matched[i, y] = true;
                }

                x = runEnd;
            }
        }
    }

    private void MarkVerticalMatches(GridSnapshot grid, bool[,] matched)
    {
        for (int x = 0; x < grid.Width; x++)
        {
            int y = 0;
            while (y < grid.Height)
            {
                var startCell = grid.Get(x, y);
                if (startCell == null)
                {
                    y++;
                    continue;
                }

                int runEnd = y + 1;
                while (runEnd < grid.Height)
                {
                    var candidate = grid.Get(x, runEnd);
                    if (candidate == null || candidate.Type != startCell.Type)
                        break;

                    runEnd++;
                }

                int runLength = runEnd - y;
                if (runLength >= 3)
                {
                    for (int i = y; i < runEnd; i++)
                        matched[x, i] = true;
                }

                y = runEnd;
            }
        }
    }
}