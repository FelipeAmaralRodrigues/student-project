using MassTransit;
using MediatR;

namespace StudentProject.Domain.Mediator.Messages
{
    public abstract class Event : Message, INotification
    {
        public Guid UId { get; set; }
        public string Message { get; set; }
        public Guid AggregateId { get; protected set; }
        public string AggregateEntityName { get; set; }
        public object Data { get; set; }

        protected Event(object data) : this()
        {
            Data = data;
            UId = NewId.NextGuid();
            SetTimestamp(DateTime.UtcNow);
        }

        protected Event()
        {
            UId = NewId.NextGuid();
            SetTimestamp(DateTime.UtcNow);
        }
    }
}
