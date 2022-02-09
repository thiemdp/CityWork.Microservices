using MassTransit;
using CityWork.Domain.Events;

namespace CityWork.Services.AuditLog.API.Consumers
{
    public class AuditLogEventConsumer : IConsumer<AuditLogEvent>
    {
        private readonly ILogger<AuditLogEventConsumer> _logger;
        private readonly IRepository<AuditLog, Guid> _repository;
        public AuditLogEventConsumer(IRepository<AuditLog, Guid> repository, ILogger<AuditLogEventConsumer> logger)
        {
            _repository = repository;
            _logger = logger;
        }
        public async Task Consume(ConsumeContext<AuditLogEvent> context)
        {
            var message = context.Message;
           if (message == null)
                throw new ArgumentNullException(nameof(message));
            var auditlog = new AuditLog()
            {
                Action = message.Action,
                EventTime = message.EventTime,
                DataJson = message.DataJson,
                UserId = message.UserId,
                Description = message.Description,
                ObjectId = message.ObjectId,
                TypeOf = message.TypeOf,
                DataJson_Old = message.DataJson_Old
            };
            await _repository.InsertAsync(auditlog);
            await _repository.UnitOfWork.SaveChangesAsync();
        }
    }
}
