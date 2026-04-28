using System.Collections.Generic;
using UnityEngine;

public struct MatchGroup
{
    public readonly List<Vector2Int> Cells;

    public MatchGroup(List<Vector2Int> cells)
    {
        Cells = cells;
    }
}