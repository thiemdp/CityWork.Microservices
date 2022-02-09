using System.ComponentModel.DataAnnotations;

namespace CityWork.Services.AuditLog.API
{
    public class AuditLog:AggregateRoot<Guid>
    {
        [MaxLength(100)]    
        public string? UserId { get; set; }
        [MaxLength(255)]
        public string? Action { get; set; }
        [MaxLength(50)]
        public string? ObjectId { get; set; }
        [MaxLength(250)]
        public string? TypeOf { get; set; }
        public string? DataJson { get; set; }
        public string? DataJson_Old { get; set; }
        [MaxLength(500)]
        public string? Description { get; set; }
        public DateTime EventTime { get; set; }
    }
}
