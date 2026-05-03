using System.Collections.Generic;

public struct MatchesResolvedEvent
{
    public IReadOnlyList<MatchGroup> Matches { get; }

    public MatchesResolvedEvent(IReadOnlyList<MatchGroup> matches)
    {
        Matches = matches;
    }
}