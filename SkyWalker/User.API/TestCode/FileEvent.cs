using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventBus;
using System.IO;
namespace User.API.TestCode
{
    public class FileEvent : IEvent
    {   
        public FileEvent(string fileName)
        {
            this.Id = Guid.NewGuid();
            this.Timestamp = DateTime.UtcNow;
            this.FileName = fileName;
        }
        public Guid Id { get; }

        public DateTime Timestamp { get; }
        public string FileName { get; }
    }
    public class FileCreateEventHandler : IEventHandler<FileEvent>
    {
        public bool CanHandle(IEvent @event)
        => @event.GetType().Equals(typeof(FileEvent));

        public Task<bool> HandleAsync(FileEvent @event, CancellationToken cancellationToken = default(CancellationToken))
        {
            File.Create(@"C:\Users\夜莫白\Desktop\qweee.text");
            return Task.FromResult(true);
        }

        public Task<bool> HandleAsync(IEvent @event, CancellationToken cancellationToken = default(CancellationToken))
        => CanHandle(@event) ? HandleAsync((FileEvent)@event, cancellationToken) : Task.FromResult(false);
    }
}
