using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityWork.Domain.Events
{
    public class AuditLogEvent
    {
        [MaxLength(100)]
        public string? UserId { get; set; }
        [MaxLength(255)]
        public string? Action { get; set; }
        [MaxLength(50)]
        public string? ObjectId { get; set; }
        [MaxLength(250)]
        public string? TypeOf { get; set; }
        public string? DataJson_Old { get; set; }
        public string? DataJson { get; set; }
        [MaxLength(500)]
        public string? Description { get; set; }
        public DateTime EventTime { get; set; }
    }
}
