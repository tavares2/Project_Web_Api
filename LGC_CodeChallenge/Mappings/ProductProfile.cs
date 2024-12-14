using AutoMapper;
using LGC_Code_Challenge.Contracts;
using LGC_CodeChallenge.Models;


namespace LGC_CodeChallenge.Mappings
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductResponse>();
            CreateMap<ProductRequest, Product>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());  // Ensure Id is not overwritten
        }
    }
}
