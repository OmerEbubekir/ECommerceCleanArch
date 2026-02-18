using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using AutoMapper;
using Core.Entities;

namespace Application.Helpers
{
    public class MappingProfiles:Profile
    {
        public MappingProfiles()
        {
            // CreateMap<Kaynak, Hedef>();
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
                .ReverseMap();

            CreateMap<CreateProductDto, Product>();
            CreateMap<UpdateProductDto, Product>();
        }
    }
}
