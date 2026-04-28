using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGridFactory 
{
    GridModel Create(int width, int height);
}
