using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace TechnicalAssessment.Domain;

public static class DomainEvents
{
    private static readonly ConcurrentDictionary<Type, List<Delegate>> _handlers = new();

    public static void Subscribe<T>(Action<T> handler)
    {
        var list = _handlers.GetOrAdd(typeof(T), _ => new List<Delegate>());
        lock (list)
        {
            list.Add(handler);
        }
    }

    public static void Raise<T>(T @event)
    {
        if (_handlers.TryGetValue(typeof(T), out var list))
        {
            Delegate[] snapshot;
            lock (list)
            {
                snapshot = list.ToArray();
            }

            foreach (var d in snapshot.OfType<Action<T>>())
            {
                try
                {
                    d(@event);
                }
                catch
                {
                    // Swallow exceptions from handlers to avoid interrupting flow
                }
            }
        }
    }

    public static void ClearAll()
    {
        _handlers.Clear();
    }
}
