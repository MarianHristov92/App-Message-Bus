using System;
using RealDigital.AppMessageBus.Interfaces;

namespace RealDigital.AppMessageBus
{
    /// <summary>
    /// Represents a callback subscription to a message type
    /// </summary>
    /// <typeparam name="T">Message Type</typeparam>
    public class Subscription<T> : ISubscription<T> where T : IMessage
    {
        /// <summary>
        /// The callback to be executed
        /// </summary>
        public Action<T> ActionHandler { get; private set; }

        /// <summary>
        /// Initializes a new subscription
        /// </summary>
        /// <param name="action">Callback</param>
        public Subscription(Action<T> action)
        {
            ActionHandler = action;
        }
    }
}
