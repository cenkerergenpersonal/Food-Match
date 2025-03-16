using System;
using System.Collections.Generic;

namespace FoodMatch.Scripts.Common
{
    public class EventBus
    {
        private static readonly Dictionary<Type, List<Delegate>> Events = new();

        public static void Subscribe<T>(Action<T> subscriber) where T : class, IEvent
        {
            Type eventType = typeof(T);
            if (!Events.ContainsKey(eventType))
            {
                Events[eventType] = new List<Delegate>();
            }

            Events[eventType].Add(subscriber);
        }

        public static void Unsubscribe<T>(Action<T> subscriber) where T : class, IEvent
        {
            Type eventType = typeof(T);

            if (Events.TryGetValue(eventType, out var delegates))
            {
                delegates.Remove(subscriber);

                if (delegates.Count == 0)
                {
                    Events.Remove(eventType);
                }
            }
        }

        public static void Publish<T>(T data) where T : class, IEvent
        {
            Type eventType = typeof(T);

            if (!Events.ContainsKey(eventType))
            {
                return;
            }
            
            foreach (var del in Events[eventType])
            {
                if (del is Action<T> action)
                {
                    action.Invoke(data);
                }
            }
        }
    }

}