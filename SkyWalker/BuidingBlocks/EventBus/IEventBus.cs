﻿using System;
using System.Collections.Generic;
using System.Text;

namespace EventBus
{
   public interface IEventBus:IEventPublisher,IEventSubscriber
    {
    }
}
