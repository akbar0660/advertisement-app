using AdvertisementApp.Dtos;
using AdvertisementApp.Entities;
using AutoMapper;

namespace AdvertisementApp.Business.Mappings.AutoMapper
{
    public class AdvertisementAppUserProfile : Profile
    {
        public AdvertisementAppUserProfile()
        {
            CreateMap<AdvertisementAppUserCreateDto, AdvertisementAppUser>().ReverseMap();
            CreateMap<AdvertisementAppUserListDto, AdvertisementAppUser>().ReverseMap();
        }
    }
}
