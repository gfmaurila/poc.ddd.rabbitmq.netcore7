using Application.DTOs;
using AutoMapper;
using Domain.Core.Entities;

namespace ApplicationTest.Fixture;

public class MappingProfileTest : Profile
{
    public MappingProfileTest()
    {
        CreateMap<User, UserDto>().ReverseMap();
        CreateMap<UserDto, User>().ReverseMap();

        CreateMap<User, UserListDto>().ReverseMap();
        CreateMap<UserListDto, User>().ReverseMap();

        CreateMap<UserDto, UserListDto>().ReverseMap();
        CreateMap<UserListDto, UserDto>().ReverseMap();
    }
}
