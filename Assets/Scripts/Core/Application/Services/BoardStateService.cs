public class BoardStateService : IBoardStateService
{
    private static readonly GridPosition[] Directions =
    {
        GridPosition.Right,
        GridPosition.Down
    };

    public bool HasPossibleMove(GridSnapshot grid)
    {
        var board = BuildBoard(grid);

        for (int y = 0; y < grid.Height; y++)
        {
            for (int x = 0; x < grid.Width; x++)
            {
                var from = new GridPosition(x, y);
                if (board[x, y] == null)
                    continue;

                for (int i = 0; i < Directions.Length; i++)
                {
                    var to = from + Directions[i];
                    if (!GridRules.IsValidPosition(to, grid.Width, grid.Height))
                        continue;

                    if (board[to.X, to.Y] == null || board[x, y] == board[to.X, to.Y])
                        continue;

                    Swap(board, from, to);

                    var createsMatch = HasMatchAt(board, from) || HasMatchAt(board, to);

                    Swap(board, from, to);

                    if (createsMatch)
                        return true;
                }
            }
        }

        return false;
    }

    private static TileType?[,] BuildBoard(GridSnapshot grid)
    {
        var board = new TileType?[grid.Width, grid.Height];

        for (int x = 0; x < grid.Width; x++)
        {
            for (int y = 0; y < grid.Height; y++)
            {
                board[x, y] = grid.Get(x, y)?.Type;
            }
        }

        return board;
    }

    private static void Swap(TileType?[,] board, GridPosition a, GridPosition b)
    {
        var temp = board[a.X, a.Y];
        board[a.X, a.Y] = board[b.X, b.Y];
        board[b.X, b.Y] = temp;
    }

    private static bool HasMatchAt(TileType?[,] board, GridPosition position)
    {
        var type = board[position.X, position.Y];
        if (type == null)
            return false;

        return CountLine(board, position, -1, 0, type.Value) + CountLine(board, position, 1, 0, type.Value) + 1 >= 3 ||
               CountLine(board, position, 0, -1, type.Value) + CountLine(board, position, 0, 1, type.Value) + 1 >= 3;
    }

    private static int CountLine(TileType?[,] board, GridPosition start, int stepX, int stepY, TileType type)
    {
        var width = board.GetLength(0);
        var height = board.GetLength(1);
        var count = 0;
        var x = start.X + stepX;
        var y = start.Y + stepY;

        while (x >= 0 && x < width && y >= 0 && y < height && board[x, y] == type)
        {
            count++;
            x += stepX;
            y += stepY;
        }

        return count;
    }
}
