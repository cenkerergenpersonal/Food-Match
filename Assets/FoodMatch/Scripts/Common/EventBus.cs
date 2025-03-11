using System;
using System.Collections.Generic;

namespace FoodMatch.Scripts.Common
{
    public class EventBus
    {
        private static readonly Dictionary<Type,  List<IEvent>> Events = new();

        public static void Subscribe<T>(Action<T> subscriber) where T : class, IEvent
        {
            Type eventType = typeof(T);
            if (!Events.ContainsKey(eventType))
            {
                Events[eventType] =new List<IEvent>();
            }
            else
            {
                Events[eventType].Add(subscriber as IEvent);
            }
        }

        public static void Unsubscribe<T>(Action<T> subscriber) where T : class, IEvent
        {
            Type eventType = typeof(T);

            if (Events.TryGetValue(eventType, out var delegates))
            {
                delegates.Remove(subscriber as IEvent);

                if (delegates.Count == 0)
                {
                    Events.Remove(eventType);
                }
            }
        }

        public static void Publish<T>(T data) where T : class
        {
            Type eventType = typeof(T);

            if (Events.TryGetValue(eventType, out var events))
            {
                foreach (var del in events)
                {
                    if (del is Action<T> action)
                    {
                        action.Invoke(data);
                    }
                }
            }
        }
    }

}