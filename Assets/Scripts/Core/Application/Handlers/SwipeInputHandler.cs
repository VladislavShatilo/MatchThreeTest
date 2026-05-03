using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using VContainer;

public class SwipeInputHandler : ISwipeInputHandler
{
    private ISwapCommandFactory swapCommandFactory;

    [Inject]
    private void Construct(ISwapCommandFactory swapCommandFactory)
    {
        this.swapCommandFactory = swapCommandFactory ?? throw new ArgumentNullException(nameof(swapCommandFactory));
    }

    public UniTask HandleSwipe(SwipeIntent intent, CancellationToken token)
    {
        var targetCell = intent.StartCell + ToOffset(intent.Direction);
        var command = swapCommandFactory.Create(new SwapRequest(intent.StartCell, targetCell));
        return command.Execute(token);
    }

    private static GridPosition ToOffset(SwipeDirection direction)
    {
        return direction switch
        {
            SwipeDirection.Left => GridPosition.Left,
            SwipeDirection.Right => GridPosition.Right,
            SwipeDirection.Up => GridPosition.Up,
            _ => GridPosition.Down
        };
    }
}
