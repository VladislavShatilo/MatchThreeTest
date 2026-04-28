using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using VContainer;

public class SwapCommandHandler : ISwapCommandHandler
{
    private IGridService gridService;
    private IEventBus eventBus;
    private IEventAwaiter eventAwaiter;
    private IMatchResolutionService matchResolutionService;
    private IMatchFinderService matchFinderService;
    private IInputLockService inputLockService;


    [Inject]
    private void Construct( IGridService gridService,IEventBus eventBus, IEventAwaiter eventAwaiter, IMatchResolutionService matchResolutionService,
        IMatchFinderService matchFinderService, IInputLockService inputLockService)
    {
        this.gridService = gridService ?? throw new ArgumentNullException(nameof(gridService));
        this.eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        this.eventAwaiter = eventAwaiter ?? throw new ArgumentNullException(nameof(eventAwaiter));
        this.matchResolutionService = matchResolutionService ?? throw new ArgumentNullException(nameof(matchResolutionService));
        this.matchFinderService = matchFinderService ?? throw new ArgumentNullException(nameof(matchFinderService));
        this.inputLockService = inputLockService ?? throw new ArgumentNullException(nameof(inputLockService));
    }

    public async UniTask Execute(SwapCommand command, CancellationToken ct)
    {
        var from = command.Request.From;
        var to = command.Request.To;

        if (!gridService.CanSwap(from, to))
            return;

        inputLockService.Lock();

        try
        {
            var swapAnimationCompleted = eventAwaiter.WaitAsync<SwapAnimationCompletedEvent>(ct);
            eventBus.Publish(new SwapRequestedEvent(from, to));
            await swapAnimationCompleted;

            gridService.Swap(from, to);
            var matches = matchFinderService.FindMatches(new GridSnapshot(gridService.Grid));

            if (matches.Count == 0)
            {
                var rollbackAnimationCompleted = eventAwaiter.WaitAsync<SwapAnimationCompletedEvent>(ct);
                eventBus.Publish(new SwapRequestedEvent(to, from));
                await rollbackAnimationCompleted;
                gridService.Swap(to, from);
                return;
            }

            await matchResolutionService.Resolve(gridService.Grid, ct);
        }
        finally
        {
            inputLockService.Unlock();
        }
    }
}