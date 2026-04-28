using System;
using UnityEngine;

public class GridService : IGridService
{
    public GridModel Grid { get; private set; }

    private readonly TileType[] AllTiles = (TileType[])Enum.GetValues(typeof(TileType));
    private readonly System.Random random = new System.Random();

    public void Initialize(int width, int height)
    {
        Grid = new GridModel(width, height);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Grid.Set(x, y, new Cell(x, y, GetRandomTile()));
            }
        }
    }

    public bool CanSwap(Vector2Int from, Vector2Int to)
    {
        return GridRules.IsValidPosition(from, Grid.Width,Grid.Height) &&
               GridRules.IsValidPosition(to, Grid.Width, Grid.Height);
    }

    public void Swap(Vector2Int a, Vector2Int b)
    {
        var temp = Grid.Get(a.x, a.y);

        Grid.Set(a.x, a.y, Grid.Get(b.x, b.y));
        Grid.Set(b.x, b.y, temp);
    }

    private TileType GetRandomTile()
    {
        return AllTiles[random.Next(AllTiles.Length)];
    }
}