using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    public TileType Type { get; }

    public Cell(TileType type)
    {
        Type = type;
    }
}