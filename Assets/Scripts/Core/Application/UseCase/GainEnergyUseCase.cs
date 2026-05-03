using VContainer;
using System;
public class GainEnergyUseCase : IGainEnergyUseCase
{
    private IEnergyStateWriter state;

    [Inject]
    private void Construct(IEnergyStateWriter state)
    {
        this.state = state ?? throw new ArgumentNullException(nameof(state));
    }

    public void Execute(int matchCount)
    {
        var energyCalculator = new EnergyCalculator();
        int reward = energyCalculator.CalculateForMatch(matchCount);
        state.Add(reward);
    }
}
