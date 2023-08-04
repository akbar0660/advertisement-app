using AdvertisementApp.Dtos;
using AdvertisementApp.Entities;
using AutoMapper;

namespace AdvertisementApp.Business.Mappings.AutoMapper
{
    public class AppUserProfile : Profile
    { 
        public AppUserProfile()
        {
            CreateMap<AppUserListDto, AppUser>().ReverseMap();
            CreateMap<AppUserCreateDto, AppUser>().ReverseMap();
            CreateMap<AppUserUpdateDto, AppUser>().ReverseMap();
        }
    }
}
