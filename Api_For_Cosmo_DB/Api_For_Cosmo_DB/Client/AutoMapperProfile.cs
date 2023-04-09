using Api_For_Cosmo_DB.DTOs;
using Api_For_Cosmo_DB.Model;
using AutoMapper;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<NameModel, NameDTO>();
    }
}