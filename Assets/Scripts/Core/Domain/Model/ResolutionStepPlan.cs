using System.Collections.Generic;

public class ResolutionStepPlan
{
    public List<TileMove> GravityMoves = new();
    public List<TileMove> SpawnMoves = new();
    public List<GridPosition> RemovedCells = new();
}
