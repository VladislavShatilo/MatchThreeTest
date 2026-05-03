using System.Collections.Generic;

public interface IMatchFinderService
{
    List<MatchGroup> FindMatches(GridSnapshot grid);
}
