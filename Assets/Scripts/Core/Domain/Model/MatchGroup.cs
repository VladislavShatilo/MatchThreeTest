using System.Collections.Generic;

public struct MatchGroup
{
    public readonly List<GridPosition> Cells;

    public MatchGroup(List<GridPosition> cells)
    {
        Cells = cells;
    }
}
