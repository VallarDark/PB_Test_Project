using AutoMapper;
using Domain.Agregates.UserAgregate;
using Persistence.Entities;

namespace Persistence.Mapper
{
    public class RepositoryProfile : Profile
    {
        public RepositoryProfile()
        {
            CreateMap<User, UserEntity>()
            .ForMember(ue => ue.Email, act => act.MapFrom(u => u.PersonalData.Email))
            .ForMember(ue => ue.Name, act => act.MapFrom(u => u.PersonalData.Name))
            .ForMember(ue => ue.LastName, act => act.MapFrom(u => u.PersonalData.LastName));

            CreateMap<UserEntity, User>()
            .ForPath(u => u.PersonalData.Email, act => act.MapFrom(ue => ue.Email))
            .ForPath(u => u.PersonalData.Name, act => act.MapFrom(ue => ue.Name))
            .ForPath(u => u.PersonalData.LastName, act => act.MapFrom(ue => ue.LastName));


            CreateMap<UserRole, UserRoleEntity>()
            .ForMember(ue => ue.Users, act => act.Ignore())
            .ReverseMap();

            CreateMap<UserEntity, UserDto>()
            .ReverseMap();

            CreateMap<UserRoleEntity, UserRoleDto>()
            .ReverseMap();

        }
    }
}
