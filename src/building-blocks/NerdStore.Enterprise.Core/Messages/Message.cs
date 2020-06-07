using System;

namespace NerdStore.Enterprise.Core.Messages
{
    public abstract class Message
    {
        protected Message()
        {
            MessageType = GetType().Name;
        }

        public string MessageType { get; set; }

        public Guid AggregateId { get; set; }
    }
}