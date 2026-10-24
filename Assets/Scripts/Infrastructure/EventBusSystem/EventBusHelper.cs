using System;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure
{
    internal static class EventBusHelper
    {
        private static Dictionary<Type, List<Type>> _cashedSubscriberTypes = new();

        public static List<Type> GetSubscriberTypes(IGlobalSubscriber globalSubscriber)
        {
            Type type = globalSubscriber.GetType();
            
            if (_cashedSubscriberTypes.TryGetValue(type, out var types))
            {
                return types;
            }

            List<Type> subscriberTypes = type
                .GetInterfaces()
                .Where(t => t.GetInterfaces()
                    .Contains(typeof(IGlobalSubscriber)))
                .ToList();

            _cashedSubscriberTypes[type] = subscriberTypes;
            return subscriberTypes;
        }
    }
}