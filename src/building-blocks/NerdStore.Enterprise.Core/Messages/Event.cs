using System;
using MediatR;

namespace NerdStore.Enterprise.Core.Messages
{
    public class Event : Message, INotification 
    {
        public Event()
        {
            TimeStamp = DateTime.Now;
        }

        public DateTime TimeStamp { get; private set; }
    }
}