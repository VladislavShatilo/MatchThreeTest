public interface IGridRaycaster
{
    bool TryGetGridPosition(ScreenPoint screenPoint, out GridPosition position);
}
