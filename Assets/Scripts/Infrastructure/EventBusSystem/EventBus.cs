using System;
using System.Collections.Generic;
using UnityEngine;

namespace Infrastructure
{
    public static class EventBus
    {
        private static Dictionary<Type, SubscribersList<IGlobalSubscriber>> _subscribers
            = new Dictionary<Type, SubscribersList<IGlobalSubscriber>>();
        
        public static void Subscribe(IGlobalSubscriber subscriber)
        {
            List<Type> subscriberTypes = EventBusHelper.GetSubscriberTypes(subscriber);
            
            foreach (Type type in subscriberTypes)
            {
                _subscribers.TryAdd(type, new SubscribersList<IGlobalSubscriber>());
                _subscribers[type].Add(subscriber);
            }
        }
        
        public static void Unsubscribe(IGlobalSubscriber subscriber)
        {
            List<Type> subscriberTypes = EventBusHelper.GetSubscriberTypes(subscriber);
            
            foreach (Type type in subscriberTypes)
            {
                if (_subscribers.TryGetValue(type, out var subscriberItem))
                    subscriberItem.Remove(subscriber);
            }
        }
        
        public static void RaiseEvent<TSubscriber>(Action<TSubscriber> action)
            where TSubscriber : class, IGlobalSubscriber
        {
            if (_subscribers.Count == 0)
            {
                Debug.Log("There are no subscribers");
                return;
            }
            
            SubscribersList<IGlobalSubscriber> subscribers = _subscribers.GetValueOrDefault(typeof(TSubscriber));

            if (subscribers == null)
            {
                return;
            }
            
            subscribers.Executing = true;
            
            foreach (IGlobalSubscriber subscriber in subscribers.TSubscribers)
            {
                try
                {
                    action.Invoke(subscriber as TSubscriber);
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
            }
            
            subscribers.Executing = false;
            subscribers.Cleanup();
        }
    }
}