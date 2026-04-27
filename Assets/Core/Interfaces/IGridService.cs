using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGridService 
{
    GridModel Grid { get; }

    void Initialize(int width, int height);
}
