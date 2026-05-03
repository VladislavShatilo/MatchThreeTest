using NUnit.Framework;

public class BoardStateServiceTests
{
    private readonly BoardStateService boardStateService = new();

    [Test]
    public void HasPossibleMove_ReturnsTrue_WhenBoardContainsValidSwap()
    {
        var grid = new GridModel(3, 3);
        grid.Set(0, 0, new Cell(TileType.Apple));
        grid.Set(1, 0, new Cell(TileType.Banana));
        grid.Set(2, 0, new Cell(TileType.Apple));
        grid.Set(0, 1, new Cell(TileType.Pear));
        grid.Set(1, 1, new Cell(TileType.Apple));
        grid.Set(2, 1, new Cell(TileType.Grape));
        grid.Set(0, 2, new Cell(TileType.Banana));
        grid.Set(1, 2, new Cell(TileType.Grape));
        grid.Set(2, 2, new Cell(TileType.Orange));

        var hasMove = boardStateService.HasPossibleMove(new GridSnapshot(grid));

        Assert.That(hasMove, Is.True);
    }

    [Test]
    public void HasPossibleMove_ReturnsFalse_WhenBoardHasNoValidSwap()
    {
        var grid = new GridModel(3, 3);
        grid.Set(0, 0, new Cell(TileType.Apple));
        grid.Set(1, 0, new Cell(TileType.Banana));
        grid.Set(2, 0, new Cell(TileType.Grape));
        grid.Set(0, 1, new Cell(TileType.Pear));
        grid.Set(1, 1, new Cell(TileType.Orange));
        grid.Set(2, 1, new Cell(TileType.Banana));
        grid.Set(0, 2, new Cell(TileType.Grape));
        grid.Set(1, 2, new Cell(TileType.Pear));
        grid.Set(2, 2, new Cell(TileType.Orange));

        var hasMove = boardStateService.HasPossibleMove(new GridSnapshot(grid));

        Assert.That(hasMove, Is.False);
    }
}
