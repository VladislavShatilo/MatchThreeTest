using Cysharp.Threading.Tasks;
using System;
using System.Threading;

public class SwapCommand : ISwapCommand
{
    private readonly SwapRequest request;
    private readonly IGridService gridService;
    private readonly IInputLockService inputLockService;
    private readonly ISwapMoveService swapMoveService;
    private readonly IMatchResolutionService matchResolutionService;
    
    
    public SwapCommand(SwapRequest request,IGridService gridService, IInputLockService inputLockService,
        ISwapMoveService swapMoveService,IMatchResolutionService matchResolutionService)
    {
        this.request = request;
        this.gridService = gridService ?? throw new ArgumentNullException(nameof(gridService));
        this.inputLockService = inputLockService ?? throw new ArgumentNullException(nameof(inputLockService));
        this.swapMoveService = swapMoveService ?? throw new ArgumentNullException(nameof(swapMoveService));
        this.matchResolutionService = matchResolutionService ?? throw new ArgumentNullException(nameof(matchResolutionService));
    }

    public async UniTask Execute(CancellationToken token)
    {
        if (!gridService.CanSwap(request.From, request.To))
            return;

        inputLockService.Lock();
        try
        {
            var swapAccepted = await swapMoveService.TryExecute(request, token);
            if (!swapAccepted)
                return;

            await matchResolutionService.Resolve(gridService.Grid, token);
        }
        finally
        {
            inputLockService.Unlock();
        }
    }
}
