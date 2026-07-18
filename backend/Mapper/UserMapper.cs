using static System.Runtime.InteropServices.JavaScript.JSType;
namespace backend.Mapper;

using AutoMapper;
using backend.model;

public class UserMapper : Profile
{
    public UserMapper()
    {
        CreateMap<User, UserResponse>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id));

        CreateMap<RegisterRequest, User>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());
    }
}
