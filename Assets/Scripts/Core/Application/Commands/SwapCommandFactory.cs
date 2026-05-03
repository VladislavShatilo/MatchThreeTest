using System;
using VContainer;

public class SwapCommandFactory : ISwapCommandFactory
{
    private IGridService gridService;
    private IInputLockService inputLockService;
    private ISwapMoveService swapMoveService;
    private IMatchResolutionService matchResolutionService;

    [Inject]
    private void Construct(IGridService gridService,IInputLockService inputLockService,ISwapMoveService swapMoveService,
        IMatchResolutionService matchResolutionService)
    {
        this.gridService = gridService ?? throw new ArgumentNullException(nameof(gridService));
        this.inputLockService = inputLockService ?? throw new ArgumentNullException(nameof(inputLockService));
        this.swapMoveService = swapMoveService ?? throw new ArgumentNullException(nameof(swapMoveService));
        this.matchResolutionService = matchResolutionService ?? throw new ArgumentNullException(nameof(matchResolutionService));
    }

    public ISwapCommand Create(SwapRequest request)
    {
        return new SwapCommand(request,gridService,inputLockService,
            swapMoveService,matchResolutionService);
    }
}
