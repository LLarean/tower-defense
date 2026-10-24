using System.Collections.Generic;

namespace Infrastructure
{
    internal class SubscribersList<TSubscriber> where TSubscriber : class
    {
        public readonly List<TSubscriber> TSubscribers = new List<TSubscriber>();
        public bool Executing;
        private bool _needsCleanUp = false;
        
        public void Add(TSubscriber subscriber)
        {
            TSubscribers.Add(subscriber);
        }

        public void Remove(TSubscriber subscriber)
        {
            if (Executing)
            {
                var index = TSubscribers.IndexOf(subscriber);
                
                if (index >= 0)
                {
                    _needsCleanUp = true;
                    TSubscribers[index] = null;
                }
            }
            else
            {
                TSubscribers.Remove(subscriber);
            }
        }

        public void Cleanup()
        {
            if (!_needsCleanUp)
            {
                return;
            }

            TSubscribers.RemoveAll(s => s == null);
            _needsCleanUp = false;
        }
    }
}