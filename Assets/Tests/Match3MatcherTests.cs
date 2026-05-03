using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

public class Match3MatcherTests
{
    private static readonly Match3Matcher Matcher = new();

    [Test]
    public void FindMatches_DetectsSimpleHorizontalMatch()
    {
        var grid = Match3Grid.FromRows(new TileType?[,]
        {
            { TileType.Apple, TileType.Apple, TileType.Apple, TileType.Banana },
            { TileType.Banana, TileType.Grape, TileType.Orange, TileType.Pear },
            { TileType.Grape, TileType.Orange, TileType.Pear, TileType.Banana }
        });

        var matches = Matcher.FindMatches(grid);

        Assert.That(matches, Has.Count.EqualTo(1));
        Assert.That(ToCoordinateSet(matches[0]), Is.EquivalentTo(new[]
        {
            "0,0",
            "1,0",
            "2,0"
        }));
    }

    [Test]
    public void FindMatches_DetectsMultipleIndependentMatches()
    {
        var grid = Match3Grid.FromRows(new TileType?[,]
        {
            { TileType.Apple, TileType.Apple, TileType.Apple, TileType.Pear, TileType.Banana },
            { TileType.Banana, TileType.Grape, TileType.Orange, TileType.Pear, TileType.Banana },
            { TileType.Orange, TileType.Grape, TileType.Banana, TileType.Orange, TileType.Banana },
            { TileType.Pear, TileType.Banana, TileType.Orange, TileType.Apple, TileType.Banana }
        });

        var matches = Matcher.FindMatches(grid);
        var groups = matches.Select(ToCoordinateSet).ToList();

        Assert.That(matches, Has.Count.EqualTo(2));
        AssertGroupExists(groups, "0,0", "1,0", "2,0");
        AssertGroupExists(groups, "4,0", "4,1", "4,2", "4,3");
    }

    [Test]
    public void FindMatches_ReturnsEmptyWhenNoMatchesExist()
    {
        var grid = Match3Grid.FromRows(new TileType?[,]
        {
            { TileType.Apple, TileType.Banana, TileType.Grape, TileType.Orange },
            { TileType.Banana, TileType.Grape, TileType.Orange, TileType.Pear },
            { TileType.Grape, TileType.Orange, TileType.Pear, TileType.Apple },
            { TileType.Orange, TileType.Pear, TileType.Apple, TileType.Banana }
        });

        var matches = Matcher.FindMatches(grid);

        Assert.That(matches, Is.Empty);
    }

    [Test]
    public void FindMatches_MergesIntersectingLinesIntoSingleGroup()
    {
        var grid = Match3Grid.FromRows(new TileType?[,]
        {
            { TileType.Banana, TileType.Apple, TileType.Banana, TileType.Pear },
            { TileType.Apple, TileType.Apple, TileType.Apple, TileType.Grape },
            { TileType.Banana, TileType.Apple, TileType.Grape, TileType.Pear },
            { TileType.Pear, TileType.Apple, TileType.Banana, TileType.Orange }
        });

        var matches = Matcher.FindMatches(grid);

        Assert.That(matches, Has.Count.EqualTo(1));
        Assert.That(ToCoordinateSet(matches[0]), Is.EquivalentTo(new[]
        {
            "1,0",
            "0,1",
            "1,1",
            "2,1",
            "1,2",
            "1,3"
        }));
    }

    [Test]
    public void FindMatches_DoesNotTreatGappedLineAsMatch()
    {
        var grid = Match3Grid.FromRows(new TileType?[,]
        {
            { TileType.Apple, TileType.Apple, null, TileType.Apple },
            { TileType.Banana, TileType.Grape, TileType.Orange, TileType.Pear },
            { TileType.Grape, TileType.Orange, TileType.Pear, TileType.Banana }
        });

        var matches = Matcher.FindMatches(grid);

        Assert.That(matches, Is.Empty);
    }

    private static HashSet<string> ToCoordinateSet(MatchGroup group)
        => group.Cells.Select(position => $"{position.X},{position.Y}").ToHashSet();

    private static void AssertGroupExists(IReadOnlyList<HashSet<string>> groups, params string[] expectedCells)
    {
        Assert.That(groups.Any(group => group.SetEquals(expectedCells)), Is.True);
    }
}
