using Cysharp.Threading.Tasks;
using System;
using System.Threading;

public interface IEventBus
{
    void Subscribe<TEvent>(Action<TEvent> handler);
    void Unsubscribe<TEvent>(Action<TEvent> handler);
    void Publish<TEvent>(TEvent eventData);

    UniTask WaitFor<T>(CancellationToken ct);

}