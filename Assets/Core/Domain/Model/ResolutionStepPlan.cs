using System.Collections.Generic;
using UnityEngine;

public class ResolutionStepPlan
{
    public List<TileMove> GravityMoves = new();
    public List<TileMove> SpawnMoves = new();
    public List<Vector2Int> RemovedCells = new();
}