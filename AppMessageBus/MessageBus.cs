using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using RealDigital.AppMessageBus.Interfaces;

namespace RealDigital.AppMessageBus
{
    /// <summary>
    /// Provides a message bus
    /// </summary>
    public class MessageBus : IMessageBus
    {
        /// <summary>
        /// Subscriptions
        /// </summary>
        private readonly Dictionary<Type, List<object>> _observers
            = new Dictionary<Type, List<object>>();

        /// <summary>
        /// Removes all subscriptions
        /// </summary>
        public void ClearAllSubscriptions()
        {
            _observers.Clear();
        }

        /// <summary>
        /// Publish a message to the bus
        /// </summary>
        /// <typeparam name="T">Message Type</typeparam>
        /// <param name="message">Message to be published</param>
        public void Publish<T>(T message) where T : IMessage
        {
            if (message == null) throw new ArgumentNullException(nameof(message));

            Type messageType = typeof(T);
            if (_observers.ContainsKey(messageType))
            {
                Task.Factory.StartNew(() =>
                {
                    var subscriptions = _observers[messageType];
                    if (subscriptions == null || subscriptions.Count == 0) return;
                    foreach (var handler in subscriptions
                        .Select(s => s as ISubscription<T>)
                        .Select(s => s.ActionHandler))
                    {
                        if (message.IsOnMainThread)
                            MainThread.BeginInvokeOnMainThread(() => { handler?.Invoke(message); });
                        else
                            handler?.Invoke(message);
                    }
                });
            }
        }

        /// <summary>
        /// Subscribe a callback for a message type
        /// </summary>
        /// <typeparam name="T">Message Type</typeparam>
        /// <param name="callback">Callback</param>
        /// <returns>Subscription object needed for unsubscribing</returns>
        public ISubscription<T> Subscribe<T>(Action<T> callback) where T : IMessage
        {
            ISubscription<T> subscription = null;

            Type messageType = typeof(T);
            var subscriptions = _observers.ContainsKey(messageType) ?
                _observers[messageType] : new List<object>();

            if (!subscriptions
                .Select(s => s as ISubscription<T>)
                .Any(s => s.ActionHandler == callback))
            {
                subscription = new Subscription<T>(callback);
                subscriptions.Add(subscription);
            }

            _observers[messageType] = subscriptions;

            return subscription;
        }

        /// <summary>
        /// Unsubscribe the specified subscription
        /// </summary>
        /// <typeparam name="T">Message Type</typeparam>
        /// <param name="subscription">Subscription to unsubscribe</param>
        /// <returns>true on success, else false</returns>
        public bool UnSubscribe<T>(ISubscription<T> subscription) where T : IMessage
        {
            bool removed = false;

            if (subscription == null) return false;

            Type messageType = typeof(T);
            if (_observers.ContainsKey(messageType))
            {
                removed = _observers[messageType].Remove(subscription);

                if (_observers[messageType].Count == 0)
                    _observers.Remove(messageType);
            }
            return removed;
        }
    }
}
