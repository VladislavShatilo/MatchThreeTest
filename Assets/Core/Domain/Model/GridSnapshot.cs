public sealed class GridSnapshot
{
    public readonly int Width;
    public readonly int Height;
    public readonly Cell[,] Cells;

    public GridSnapshot(GridModel grid)
    {
        Width = grid.Width;
        Height = grid.Height;

        Cells = new Cell[Width, Height];

        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                Cells[x, y] = grid.Get(x, y);
            }
        }    
           
    }

    public Cell Get(int x, int y) => Cells[x, y];
}