using System;

public sealed class Match3Grid
{
    private readonly TileType?[] cells;

    public int Width { get; }
    public int Height { get; }

    public Match3Grid(int width, int height, TileType?[] cells)
    {
        if (width <= 0)
            throw new ArgumentOutOfRangeException(nameof(width));

        if (height <= 0)
            throw new ArgumentOutOfRangeException(nameof(height));

        if (cells == null)
            throw new ArgumentNullException(nameof(cells));

        if (cells.Length != width * height)
            throw new ArgumentException("Cell count does not match grid size.", nameof(cells));

        Width = width;
        Height = height;
        this.cells = new TileType?[cells.Length];
        Array.Copy(cells, this.cells, cells.Length);
    }

    public TileType? Get(int x, int y) => cells[(y * Width) + x];

    public static Match3Grid FromRows(TileType?[,] rows)
    {
        if (rows == null)
            throw new ArgumentNullException(nameof(rows));

        var height = rows.GetLength(0);
        var width = rows.GetLength(1);
        var cells = new TileType?[width * height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
                cells[(y * width) + x] = rows[y, x];
        }

        return new Match3Grid(width, height, cells);
    }

    public static Match3Grid FromSnapshot(GridSnapshot snapshot)
    {
        if (snapshot == null)
            throw new ArgumentNullException(nameof(snapshot));

        var cells = new TileType?[snapshot.Width * snapshot.Height];

        for (int y = 0; y < snapshot.Height; y++)
        {
            for (int x = 0; x < snapshot.Width; x++)
            {
                var cell = snapshot.Get(x, y);
                cells[(y * snapshot.Width) + x] = cell?.Type;
            }
        }

        return new Match3Grid(snapshot.Width, snapshot.Height, cells);
    }
}
