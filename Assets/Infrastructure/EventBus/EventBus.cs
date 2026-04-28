using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class EventBus : IEventBus
{
    private readonly Dictionary<Type, List<Delegate>> eventHandlers = new();
    private readonly Dictionary<Type, List<UniTaskCompletionSource>> awaiters = new();
    public void Subscribe<TEvent>(Action<TEvent> handler)
    {
        if (handler == null)
            throw new ArgumentNullException(nameof(handler));

        var type = typeof(TEvent);

        if (!eventHandlers.TryGetValue(type, out var handlers))
        {
            handlers = new List<Delegate>();
            eventHandlers[type] = handlers;
        }

        if (!handlers.Contains(handler))
            handlers.Add(handler);
    }

    public void Unsubscribe<TEvent>(Action<TEvent> handler)
    {
        if (handler == null)
            throw new ArgumentNullException(nameof(handler));

        var type = typeof(TEvent);

        if (!eventHandlers.TryGetValue(type, out var handlers))
            return;

        handlers.Remove(handler);

        if (handlers.Count == 0)
            eventHandlers.Remove(type);
    }
    public UniTask WaitFor<T>(CancellationToken ct)
    {
        var type = typeof(T);
        var tcs = new UniTaskCompletionSource();

        if (!awaiters.TryGetValue(type, out var list))
        {
            list = new List<UniTaskCompletionSource>();
            awaiters[type] = list;
        }

        list.Add(tcs);

        ct.Register(() =>
        {
            list.Remove(tcs);
            tcs.TrySetCanceled();
        });

        return tcs.Task;
    }

    public void Publish<T>(T eventData)
    {
        var type = typeof(T);

        // обычные подписчики
        if (eventHandlers.TryGetValue(type, out var handlers))
        {
            foreach (var handler in new List<Delegate>(handlers))
            {
                ((Action<T>)handler)?.Invoke(eventData);
            }
        }

        // awaiters
        if (awaiters.TryGetValue(type, out var waiters))
        {
            foreach (var waiter in waiters)
            {
                waiter.TrySetResult();
            }

            waiters.Clear();
        }
    }

}