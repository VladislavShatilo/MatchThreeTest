using UnityEngine;

public class GridService : IGridService
{
    public GridModel Grid { get; private set; }

    public void Initialize(GridModel grid)
    {
        Grid = grid;
    }

    public void Swap(Vector2Int a, Vector2Int b)
    {
        var temp = Grid.Get(a.x, a.y);

        Grid.Set(a.x, a.y, Grid.Get(b.x, b.y));
        Grid.Set(b.x, b.y, temp);
    }

    public bool CanSwap(Vector2Int a, Vector2Int b)
    {
        return GridRules.IsNeighbor(a, b) &&
               GridRules.IsValidPosition(a, Grid.Width, Grid.Height) &&
               GridRules.IsValidPosition(b, Grid.Width, Grid.Height);
    }
}