using System;

public  struct GridPosition
{
    public static readonly GridPosition Left = new(-1, 0);
    public static readonly GridPosition Right = new(1, 0);
    public static readonly GridPosition Up = new(0, -1);
    public static readonly GridPosition Down = new(0, 1);

    public int X { get; }
    public int Y { get; }

    public GridPosition(int x, int y)
    {
        X = x;
        Y = y;
    }

    public static GridPosition operator +(GridPosition a, GridPosition b)
        => new(a.X + b.X, a.Y + b.Y);

   
}
