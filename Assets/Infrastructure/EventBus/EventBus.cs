using System;
using System.Collections.Generic;
using UnityEngine;

public class EventBus : IEventBus
{
    private readonly Dictionary<Type, List<Delegate>> eventHandlers = new();

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

    public void Publish<TEvent>(TEvent eventData)
    {
        if (eventData == null)
        {
            Debug.LogWarning($"EventBus: try to publish null event ({typeof(TEvent).Name}).");
            return;
        }

        var type = typeof(TEvent);

        if (!eventHandlers.TryGetValue(type, out var handlers) || handlers.Count == 0)
            return;

        var handlersCopy = new List<Delegate>(handlers);

        foreach (var handler in handlersCopy)
        {
            try
            {
                ((Action<TEvent>)handler)?.Invoke(eventData);
            }
            catch (Exception ex)
            {
                Debug.LogError($"EventBus: error in handler {type.Name}: {ex}");
            }
        }
    }

}