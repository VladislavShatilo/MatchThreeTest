using UnityEngine;

public interface ISwipeCommandSender
{
    void SendSwap(Vector2Int from, Vector2Int to);
}