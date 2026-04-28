using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using VContainer;

public sealed class EventAwaiter : IEventAwaiter
{
    private IEventBus eventBus;

    [Inject]
    private void Construct(IEventBus eventBus)
    {
        this.eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
    }

    public UniTask WaitAsync<T>(CancellationToken ct)
    {
        var tcs = new UniTaskCompletionSource();

        Action<T> handler = null;

        handler = (evt) =>
        {
            eventBus.Unsubscribe(handler);
            tcs.TrySetResult();
        };

        eventBus.Subscribe(handler);

        ct.Register(() =>
        {
            eventBus.Unsubscribe(handler);
            tcs.TrySetCanceled();
        });

        return tcs.Task;
    }

    public UniTask<T> WaitAsync<T>(Func<T, bool> predicate, CancellationToken ct)
    {
        var tcs = new UniTaskCompletionSource<T>();

        Action<T> handler = null;

        handler = (evt) =>
        {
            if (!predicate(evt))
                return;

            eventBus.Unsubscribe(handler);
            tcs.TrySetResult(evt);
        };

        eventBus.Subscribe(handler);

        ct.Register(() =>
        {
            eventBus.Unsubscribe(handler);
            tcs.TrySetCanceled();
        });

        return tcs.Task;
    }
}
