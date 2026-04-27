using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridService : IGridService
{
    public GridModel Grid { get; private set; }

    private readonly TileType[] AllTiles = (TileType[])Enum.GetValues(typeof(TileType));
    private System.Random random = new System.Random();

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

    private TileType GetRandomTile()
    {
        return AllTiles[random.Next(AllTiles.Length)];
    }
}