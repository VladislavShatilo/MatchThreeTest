public class GridService : IGridService
{
    public GridModel Grid { get; private set; }

    public void Initialize(GridModel grid)
    {
        Grid = grid;
    }

    public void Swap(GridPosition a, GridPosition b)
    {
        var temp = Grid.Get(a.X, a.Y);

        Grid.Set(a.X, a.Y, Grid.Get(b.X, b.Y));
        Grid.Set(b.X, b.Y, temp);
    }

    public bool CanSwap(GridPosition a, GridPosition b)
    {
        return GridRules.IsNeighbor(a, b) &&
               GridRules.IsValidPosition(a, Grid.Width, Grid.Height) &&
               GridRules.IsValidPosition(b, Grid.Width, Grid.Height);
    }
}
