using System.Collections.Generic;

public class ResolutionStepApplier : IResolutionStepApplier
{
    public void Apply(GridModel grid, ResolutionStepPlan plan)
    {
        ApplyRemovedToModel(grid, plan);
        ApplyGravityToModel(grid, plan);
        ApplySpawnToModel(grid, plan);
    }

    private void ApplyRemovedToModel(GridModel grid, ResolutionStepPlan plan)
    {
        foreach (var pos in plan.RemovedCells)
        {
            if (!GridRules.IsValidPosition(pos, grid.Width, grid.Height))
                continue;

            grid.Set(pos.X, pos.Y, null);
        }
    }

    private void ApplyGravityToModel(GridModel grid, ResolutionStepPlan plan)
    {
        foreach (var move in plan.GravityMoves)
        {
            if (!GridRules.IsValidPosition(move.From, grid.Width, grid.Height) ||
                !GridRules.IsValidPosition(move.To, grid.Width, grid.Height))
            {
                continue;
            }

            var cell = grid.Get(move.From.X, move.From.Y);
            if (cell == null)
                continue;

            grid.Set(move.From.X, move.From.Y, null);
            grid.Set(move.To.X, move.To.Y, cell);
        }
    }

    private void ApplySpawnToModel(GridModel grid, ResolutionStepPlan plan)
    {
        var occupiedTargets = new HashSet<GridPosition>();
        foreach (var spawn in plan.SpawnMoves)
        {
            if (!GridRules.IsValidPosition(spawn.To, grid.Width, grid.Height))
                continue;
            if (!occupiedTargets.Add(spawn.To))
                continue;

            var cell = new Cell(spawn.tileType);
            grid.Set(spawn.To.X, spawn.To.Y, cell);
        }
    }
}
