using System;
using System.Collections.Generic;
using System.Text;

namespace EventBus
{
    public class   EventProcessedEventArgs:EventArgs
    {
        public EventProcessedEventArgs(IEvent @event)
        {
            this.Event = @event;
        }
        public IEvent Event { get; private set; }
    }
}
