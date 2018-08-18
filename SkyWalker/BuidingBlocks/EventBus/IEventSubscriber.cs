using System;
using System.Collections.Generic;
using System.Text;

namespace EventBus
{
   public interface IEventSubscriber:IDisposable
    {
        void Subscribe();
    }
}
