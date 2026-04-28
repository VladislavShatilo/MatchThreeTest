using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Tiles/Tile Database")]
public class TileDatabase : ScriptableObject
{
    public List<TileConfig> Tiles;
}