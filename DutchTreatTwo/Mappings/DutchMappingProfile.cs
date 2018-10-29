using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DutchTreatTwo.Data.Entities;
using DutchTreatTwo.ViewModels;

namespace DutchTreatTwo.Mappings
{
    public class DutchMappingProfile : Profile
    {
       public DutchMappingProfile()
       {
          CreateMap<Order, OrderViewModel>()
             .ForMember(ovm => ovm.OrderId, src => src.MapFrom(o => o.Id))
             .ReverseMap();

          CreateMap<OrderItem, OrderItemViewModel>()
             .ReverseMap();
       }
    }
}
