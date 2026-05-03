public class GridModel
{
    private readonly Cell[,] cells;

    public int Width { get; }
    public int Height { get; }

    public GridModel(int width, int height)
    {
        Width = width;
        Height = height;
        cells = new Cell[width, height];
    }

    public void Set(int x, int y, Cell cell)
    {
        cells[x, y] = cell;
    }

    public Cell Get(int x, int y) => cells[x, y];

   
   
}
