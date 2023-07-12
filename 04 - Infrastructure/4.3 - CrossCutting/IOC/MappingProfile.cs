using Application.DTOs;
using AutoMapper;
using Domain.Core.Entities;

namespace IOC;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, UserDto>().ReverseMap();
        CreateMap<UserDto, User>().ReverseMap();
    }
}
