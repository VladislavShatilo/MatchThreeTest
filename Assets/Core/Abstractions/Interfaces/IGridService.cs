using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGridService 
{
    GridModel Grid { get; }

    void Initialize(GridModel grid);

    void Swap(Vector2Int a, Vector2Int b);

    bool CanSwap(Vector2Int from, Vector2Int to);
}
