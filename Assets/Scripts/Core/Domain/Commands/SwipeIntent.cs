public struct SwipeIntent
{
    public readonly GridPosition StartCell;
    public readonly SwipeDirection Direction;

    public SwipeIntent(GridPosition startCell, SwipeDirection direction)
    {
        StartCell = startCell;
        Direction = direction;
    }
}
