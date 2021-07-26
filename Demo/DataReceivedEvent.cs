using System;
using RealDigital.AppMessageBus.Interfaces;

namespace RealDigital.AppMessageBus.Models
{
    public class DataReceivedEvent : IMessage
    {
        public string Description { get; set; }
        public bool IsOnMainThread { get; set; }

        public DataReceivedEvent(string desc, bool main)
        {
            Description = desc;
            IsOnMainThread = main;
        }
    }
}
