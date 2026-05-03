using System.Collections.Generic;
using NUnit.Framework;

public class EnergyCalculatorTests
{
    private static readonly EnergyCalculator Calculator = new();

    [Test]
    public void CalculateForMatch_ReturnsEnergyForSingleMatch()
    {
        var energy = Calculator.CalculateForMatch(3);

        Assert.That(energy, Is.EqualTo(30));
    }

    [Test]
    public void CalculateForMatch_ReturnsZeroForNonPositiveMatchSize()
    {
        var energy = Calculator.CalculateForMatch(0);

        Assert.That(energy, Is.Zero);
    }

    [Test]
    public void CalculateTotal_SumsEnergyAcrossMultipleMatches()
    {
        var matches = new List<MatchGroup>
        {
            new(new List<GridPosition>
            {
                new(0, 0),
                new(1, 0),
                new(2, 0)
            }),
            new(new List<GridPosition>
            {
                new(4, 0),
                new(4, 1),
                new(4, 2),
                new(4, 3)
            })
        };

        var energy = Calculator.CalculateTotal(matches);

        Assert.That(energy, Is.EqualTo(70));
    }

    [Test]
    public void CalculateTotal_ReturnsZeroWhenMatchesAreAbsent()
    {
        var energy = Calculator.CalculateTotal(new List<MatchGroup>());

        Assert.That(energy, Is.Zero);
    }
}
