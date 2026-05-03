using System;
using System.Collections.Generic;

public class MatchFinderService : IMatchFinderService
{
    private readonly Match3Matcher matcher = new();

    public List<MatchGroup> FindMatches(GridSnapshot grid)
    {
        if (grid == null)
            throw new ArgumentNullException(nameof(grid));

        var matches = matcher.FindMatches(Match3Grid.FromSnapshot(grid));
        return new List<MatchGroup>(matches);
    }
}
