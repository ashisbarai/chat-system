using System;

namespace Chat.Api.Core.Events
{
    public class EventBase : IDomainEvent
    {
        protected EventBase()
        {
            OccuredOn = DateTime.Now;
        }

        public DateTime OccuredOn
        {
            get;
        }
    }
}