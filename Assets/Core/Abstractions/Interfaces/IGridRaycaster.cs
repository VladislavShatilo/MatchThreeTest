using UnityEngine;

public interface IGridRaycaster
{
    bool TryGetCellView(Vector3 screenPosition, out CellView cellView);
}