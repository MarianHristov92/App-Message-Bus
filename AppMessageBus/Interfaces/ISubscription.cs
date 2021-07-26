using System;
namespace RealDigital.AppMessageBus.Interfaces
{
    /// <summary>
    /// Defines the interface of a subscription
    /// </summary>
    /// <typeparam name="T">Message Type</typeparam>
    public interface ISubscription<T> where T : IMessage
    {
        /// <summary>
        /// Callback
        /// </summary>
        Action<T> ActionHandler { get; }
    }
}
