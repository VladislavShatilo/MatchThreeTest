using R3;

public interface IEnergyStateReader
{
    int Current { get; }
    ReadOnlyReactiveProperty<int> Observe { get; }
}
