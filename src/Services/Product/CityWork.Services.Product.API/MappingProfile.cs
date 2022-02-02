using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CityWork.Services.Product.API
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductFullResponse>();
            CreateMap<AddUpdateProductCommand, Product>();
        }
    }
}
