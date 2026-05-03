public interface IGridService 
{
    GridModel Grid { get; }

    void Initialize(GridModel grid);

    void Swap(GridPosition a, GridPosition b);

    bool CanSwap(GridPosition from, GridPosition to);
}
