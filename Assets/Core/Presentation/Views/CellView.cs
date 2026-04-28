using UnityEngine;

public class CellView : MonoBehaviour
{
    public Vector2Int GridPosition { get; private set; }

    public void SetPosition(Vector2Int pos)
    {
        GridPosition = pos;
    }
}