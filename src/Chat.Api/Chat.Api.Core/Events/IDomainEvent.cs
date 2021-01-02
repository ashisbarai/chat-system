using System;

namespace Chat.Api.Core.Events
{
    public interface IDomainEvent
    {
        DateTime OccuredOn { get; }
    }
}