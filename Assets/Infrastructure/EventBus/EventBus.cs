using System;
using System.Collections.Generic;

public class EventBus : IEventBus
{
    private readonly Dictionary<Type, List<Delegate>> handlers = new();

    public void Subscribe<T>(Action<T> handler)
    {
        var type = typeof(T);

        if (!handlers.TryGetValue(type, out var list))
        {
            list = new List<Delegate>();
            handlers[type] = list;
        }

        list.Add(handler);
    }

    public void Unsubscribe<T>(Action<T> handler)
    {
        var type = typeof(T);

        if (!handlers.TryGetValue(type, out var list))
            return;

        list.Remove(handler);

        if (list.Count == 0)
            handlers.Remove(type);
    }

    public void Publish<T>(T evt)
    {
        var type = typeof(T);

        if (!handlers.TryGetValue(type, out var list))
            return;

        for (int i = 0; i < list.Count; i++)
        {
            if (list[i] is Action<T> action)
                action(evt);
        }
    }
}