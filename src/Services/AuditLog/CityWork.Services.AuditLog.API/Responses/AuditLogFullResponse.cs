namespace CityWork.Services.AuditLog.API
{
    public class AuditLogFullResponse:AggregateRoot<Guid>
    {
        public string? UserId { get; set; }
        public string? Action { get; set; }
        public string? ObjectId { get; set; }
        public string? TypeOf { get; set; }
        public string? DataJson { get; set; }
        public string? DataJson_Old { get; set; }
        public string? Description { get; set; }
        public DateTime EventTime { get; set; }
    }
}
