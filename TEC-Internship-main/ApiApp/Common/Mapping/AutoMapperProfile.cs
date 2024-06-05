using ApiApp.Common.Dto;
using AutoMapper;
using Internship.Model;

namespace ApiApp.Common.Mapping;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Person, PersonDto>().ReverseMap();
        CreateMap<CreateUpdatePersonDto, Person>();

        CreateMap<PersonDetails, PersonDetailsDto>().ReverseMap();
        CreateMap<CreateUpdatePersonDetailsDto, PersonDetails>();

        CreateMap<Position, PositionDto>().ReverseMap();
        CreateMap<CreateUpdatePositionDto, Position>();

        CreateMap<Salary, SalaryDto>().ReverseMap();
        CreateMap<CreateUpdateSalaryDto, Salary>();

        CreateMap<Department, DepartmentDto>().ReverseMap();
        CreateMap<CreateUpdateDepartmentDto, Department>();
    }
}