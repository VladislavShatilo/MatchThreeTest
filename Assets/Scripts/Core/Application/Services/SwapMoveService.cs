using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using VContainer;

public class SwapMoveService : ISwapMoveService
{
    private IGridService gridService;
    private IMatchFinderService matchFinderService;
    private IBoardAnimationPort boardAnimationPort;

    [Inject]
    private void Construct(IGridService gridService,IMatchFinderService matchFinderService,IBoardAnimationPort boardAnimationPort)
    {
        this.gridService = gridService ?? throw new ArgumentNullException(nameof(gridService));
        this.matchFinderService = matchFinderService ?? throw new ArgumentNullException(nameof(matchFinderService));
        this.boardAnimationPort = boardAnimationPort ?? throw new ArgumentNullException(nameof(boardAnimationPort));
    }

    public async UniTask<bool> TryExecute(SwapRequest request, CancellationToken token)
    {
        await boardAnimationPort.PlaySwap(request.From, request.To, token);
        gridService.Swap(request.From, request.To);

        var matches = matchFinderService.FindMatches(new GridSnapshot(gridService.Grid));
        if (matches.Count > 0)
            return true;

        await boardAnimationPort.PlaySwap(request.To, request.From, token);
        gridService.Swap(request.To, request.From);
        return false;
    }
}
