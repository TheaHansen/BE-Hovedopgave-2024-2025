using AutoMapper;
using BE_Hovedopgave_2024_2025.DTOs;
using BE_Hovedopgave_2024_2025.Model;

namespace BE_Hovedopgave_2024_2025.Profiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Product, ProductDTO>();
    }
}