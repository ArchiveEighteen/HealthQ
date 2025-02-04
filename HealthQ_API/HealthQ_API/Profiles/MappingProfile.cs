using AutoMapper;
using HealthQ_API.DTOs;
using HealthQ_API.Entities;

namespace HealthQ_API.Profiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<UserDTO, UserModel>()
            .ForMember(dest => dest.PasswordHash,
                opt => opt.Ignore())
            .ForMember(dest => dest.PasswordSalt, 
                opt => opt.Ignore())
            .ForMember(dest => dest.Gender,
                opt => opt
                    .MapFrom(ud => Enum.Parse<EGender>(ud.Gender)))
            .ForMember(dest => dest.UserType,
                opt => opt
                    .MapFrom(ud => Enum.Parse<EUserType>(ud.UserType)));

        CreateMap<UserModel, UserDTO>()
            .ForMember(dest => dest.Password,
                opt => opt.Ignore())
            .ForMember(dest => dest.Gender,
                opt => opt
                    .MapFrom(um => um.Gender.ToString()))
            .ForMember(dest => dest.UserType,
                opt => opt
                    .MapFrom(um => um.UserType.ToString()));
    }
}