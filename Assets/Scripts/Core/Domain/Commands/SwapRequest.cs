public struct SwapRequest
{
    public readonly GridPosition From;
    public readonly GridPosition To;

    public SwapRequest(GridPosition from, GridPosition to)
    {
        From = from;
        To = to;
    }
}
