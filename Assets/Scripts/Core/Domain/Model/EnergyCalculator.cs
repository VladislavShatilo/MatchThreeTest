using System;
using System.Collections.Generic;

public sealed class EnergyCalculator
{
    public int EnergyPerDestroyedTile { get; }

    public EnergyCalculator(int energyPerDestroyedTile = 10)
    {
        if (energyPerDestroyedTile < 0)
            throw new ArgumentOutOfRangeException(nameof(energyPerDestroyedTile));

        EnergyPerDestroyedTile = energyPerDestroyedTile;
    }

    public int CalculateForMatch(int destroyedTilesCount)
    {
        if (destroyedTilesCount <= 0)
            return 0;

        return destroyedTilesCount * EnergyPerDestroyedTile;
    }

    public int CalculateTotal(IReadOnlyList<MatchGroup> matches)
    {
        if (matches == null)
            throw new ArgumentNullException(nameof(matches));

        var total = 0;
        for (int i = 0; i < matches.Count; i++)
            total += CalculateForMatch(matches[i].Cells.Count);

        return total;
    }
}
