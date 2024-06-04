using ApiApp.Common.Dto;
using AutoMapper;
using Internship.Model;

namespace ApiApp.Common.Mapping;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Person, PersonDto>().ReverseMap();
        CreateMap<CreatePersonDto, Person>();
    }
}