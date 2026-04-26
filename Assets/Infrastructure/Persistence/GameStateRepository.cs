using R3;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateRepository : IGameStateRepository
{
    private readonly ReactiveProperty<int> natureEnergy;

    public ReadOnlyReactiveProperty<int> NatureEnergy { get; }

    public GameStateRepository()
    {
        natureEnergy = new ReactiveProperty<int>(0);
        NatureEnergy = natureEnergy.ToReadOnlyReactiveProperty();
    }

    public void AddEnergy(int value)
    {
        if (value <= 0) return;

        natureEnergy.Value += value;
    }

    public void Reset()
    {
        natureEnergy.Value = 0;
    }
}
