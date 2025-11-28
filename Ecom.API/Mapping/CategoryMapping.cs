using AutoMapper;
using Ecom.Core.DTOs;
using Ecom.Core.Entities.Product;

namespace Ecom.API.Mapping;

public class CategoryMapping : Profile
{
    public CategoryMapping()
    {
        CreateMap<AddCategoryDTO, Category>().ReverseMap();
        CreateMap<UpdateCategoryDTO, Category>().ReverseMap();
    }
}
