    using UnityEngine;

public class TileTypeGenerator : ITileTypeGenerator
{
    public TileType GetRandom()
    {
        var values = System.Enum.GetValues(typeof(TileType));
        return (TileType)values.GetValue(Random.Range(0, values.Length));
    }
}