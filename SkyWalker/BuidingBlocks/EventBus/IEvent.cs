using System;
using System.Collections.Generic;
using System.Text;

namespace EventBus
{
   public interface IEvent
    {
        Guid Id { get; }
        DateTime Timestamp { get; }
    }
}
