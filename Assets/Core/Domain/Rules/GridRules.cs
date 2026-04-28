using UnityEngine;

public static class GridRules
{
    public static bool IsNeighbor(Vector2Int a, Vector2Int b)
        => Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y) == 1;

    public static bool IsValidPosition(Vector2Int pos, int width, int height)
        => pos.x >= 0 && pos.x < width &&
           pos.y >= 0 && pos.y < height;
}