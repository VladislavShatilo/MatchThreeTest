using R3;

public class EnergyState : IEnergyStateReader, IEnergyStateWriter
{
    private readonly ReactiveProperty<int> energy = new(0);

    public int Current => energy.Value;
    public ReadOnlyReactiveProperty<int> Observe => energy;

    public void Add(int value)
    {
        if (value <= 0) return;
        energy.Value += value;
    }

    public void Reset() => energy.Value = 0;
}