using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITileFactory 
{
    UniTask<GameObject> Create(TileType type);
}
