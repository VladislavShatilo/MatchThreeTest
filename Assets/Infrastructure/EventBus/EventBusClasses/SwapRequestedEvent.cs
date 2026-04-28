using UnityEngine;

public struct SwapRequestedEvent
{
    public readonly Vector2Int From;
    public readonly Vector2Int To;

    public SwapRequestedEvent(Vector2Int from, Vector2Int to)
    {
        From = from;
        To = to;
    }
}