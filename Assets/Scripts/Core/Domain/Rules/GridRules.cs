public static class GridRules
{
    public static bool IsNeighbor(GridPosition a, GridPosition b)
        => System.Math.Abs(a.X - b.X) + System.Math.Abs(a.Y - b.Y) == 1;

    public static bool IsValidPosition(GridPosition pos, int width, int height)
        => pos.X >= 0 && pos.X < width &&
           pos.Y >= 0 && pos.Y < height;
}
