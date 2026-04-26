using R3;

public interface IGameStateRepository
{
    ReadOnlyReactiveProperty<int> NatureEnergy { get; }

    void AddEnergy(int value);
    void Reset();
}