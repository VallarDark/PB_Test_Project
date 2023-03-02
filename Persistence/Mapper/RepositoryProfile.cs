using AutoMapper;
using Domain.Agregates.ProductAgregate;
using Domain.Agregates.UserAgregate;
using Persistence.Entities;

namespace Persistence.Mapper
{
    public class RepositoryProfile : Profile
    {
        public RepositoryProfile()
        {
            CreateMap<User, UserEntity>()
                .ForMember(ue => ue.Id, act => act.MapFrom(u => u.Id))
                .ForMember(ue => ue.Email, act => act.MapFrom(u => u.PersonalData.Email))
                .ForMember(ue => ue.Name, act => act.MapFrom(u => u.PersonalData.Name))
                .ForMember(ue => ue.LastName, act => act.MapFrom(u => u.PersonalData.LastName));

            CreateMap<UserEntity, User>()
                .ForMember(ue => ue.Id, act => act.MapFrom(u => u.Id))
                .ForPath(u => u.PersonalData.Email, act => act.MapFrom(ue => ue.Email))
                .ForPath(u => u.PersonalData.Name, act => act.MapFrom(ue => ue.Name))
                .ForPath(u => u.PersonalData.LastName, act => act.MapFrom(ue => ue.LastName));

            CreateMap<UserRole, UserRoleEntity>()
                .ForMember(ue => ue.Users, act => act.Ignore())
                .ReverseMap();

            CreateMap<UserEntity, UserDto>()
                .ForMember(ue => ue.Id, act => act.MapFrom(u => u.Id))
                .ReverseMap();

            CreateMap<UserRoleEntity, UserRoleDto>()
                .ForMember(ue => ue.Id, act => act.MapFrom(u => u.Id))
                .ReverseMap();

            CreateMap<Product, ProductEntity>()
                .ForMember(ue => ue.Categories, act => act.MapFrom(u => u.Categories))
                .ForMember(ue => ue.Id, act => act.MapFrom(u => u.Id))
                .PreserveReferences()
                .ReverseMap();

            CreateMap<ProductCategory, ProductCategoryEntity>()
                .ForMember(ue => ue.Products, act => act.MapFrom(u => u.Products))
                .ForMember(ue => ue.Id, act => act.MapFrom(u => u.Id))
                .PreserveReferences()
                .ReverseMap();

            CreateMap<ProductEntity, ProductDto>()
                .ForMember(ue => ue.Id, act => act.MapFrom(u => u.Id))
                .ForMember(ue => ue.Categories, act => act.MapFrom(u => u.Categories))
                .ForMember(ue => ue.CyclicDepth, act => act.MapFrom(u => 1))
                .PreserveReferences()
                .ReverseMap();

            CreateMap<ProductCategoryEntity, ProductCategoryDto>()
                .ForMember(ue => ue.Id, act => act.MapFrom(u => u.Id))
                .ForMember(ue => ue.Products, act => act.MapFrom(u => u.Products))
                .ForMember(ue => ue.CyclicDepth, act => act.MapFrom(u => 1))
                .PreserveReferences()
                .ReverseMap();
        }
    }
}
