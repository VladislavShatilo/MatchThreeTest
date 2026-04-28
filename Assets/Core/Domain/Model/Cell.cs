using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    public int X { get; }
    public int Y { get; }
    public TileType Type { get; }

    public Cell(int x, int y, TileType type)
    {
        X = x;
        Y = y;
        Type = type;
    }
}