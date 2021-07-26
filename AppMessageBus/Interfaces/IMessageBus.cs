using System;
namespace RealDigital.AppMessageBus.Interfaces
{
    /// <summary>
    /// Defines the interface of a message bus
    /// </summary>
    public interface IMessageBus
    {
        /// <summary>
        /// Publish a message to the bus
        /// </summary>
        /// <typeparam name="T">Message Type</typeparam>
        /// <param name="message">Message to be published</param>
        void Publish<T>(T message) where T : IMessage;

        /// <summary>
        /// Subscribe a callback for a message type
        /// </summary>
        /// <typeparam name="T">Message Type</typeparam>
        /// <param name="callback">Callback</param>
        /// <returns>Subscription object needed for unsubscribing</returns>
        ISubscription<T> Subscribe<T>(Action<T> actionCallback) where T : IMessage;

        /// <summary>
        /// Unsubscribe the specified subscription
        /// </summary>
        /// <typeparam name="T">Message Type</typeparam>
        /// <param name="subscription">Subscription to unsubscribe</param>
        /// <returns>true on success, else false</returns>
        bool UnSubscribe<T>(ISubscription<T> subscription) where T : IMessage;

        /// <summary>
        /// Removes all subscriptions
        /// </summary>
        void ClearAllSubscriptions();
    }
}
