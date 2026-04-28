using Cysharp.Threading.Tasks;
using System;
using System.Threading;

public interface IEventBus
{
    void Subscribe<T>(Action<T> handler);
    void Unsubscribe<T>(Action<T> handler);
    void Publish<T>(T evt);

}