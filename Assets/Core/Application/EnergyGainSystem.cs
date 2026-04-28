using System;
using VContainer;

public class EnergyGainSystem : IEnergyGainSystem
{
    private IGameStateRepository gameStateRepository;

    [Inject]
    public void Construct(IGameStateRepository gameStateRepository)
    {
        this.gameStateRepository = gameStateRepository ?? throw new ArgumentNullException(nameof(gameStateRepository));
      
    }
    public void OnMatch(int count)
    {
        gameStateRepository.AddEnergy(CalculateReward(count));
    }
    private int CalculateReward(int count)
    {
        return count * 10;
    }

}