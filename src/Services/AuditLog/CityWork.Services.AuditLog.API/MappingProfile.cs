using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CityWork.Services.AuditLog.API
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AuditLog, AuditLogFullResponse>();
        }
    }
}
