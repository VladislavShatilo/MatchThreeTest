using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using UnityEngine;
using VContainer;

public class SwapCommandHandler : ISwapCommandHandler
{
    private IGridService gridService;
    private IEventBus eventBus;

    [Inject]
    private void Construct(IGridService gridService, IEventBus eventBus)
    {
        this.gridService = gridService ?? throw new ArgumentNullException(nameof(gridService));
        this.eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
    }

    public async UniTask Execute(SwapCommand command, CancellationToken ct)
    {
        var from = command.Request.From;
        var to = command.Request.To;

        if (!gridService.CanSwap(from, to))
            return;

        eventBus.Publish(new SwapRequestedEvent(from, to));

        await eventBus.WaitFor<SwapAnimationCompletedEvent>(ct);

        gridService.Swap(from, to);

        eventBus.Publish(new SwapAcceptedEvent(command.Request));
    }
}