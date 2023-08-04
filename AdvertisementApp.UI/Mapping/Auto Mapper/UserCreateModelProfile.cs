using AdvertisementApp.Dtos;
using AdvertisementApp.UI.Models;
using AutoMapper;

namespace AdvertisementApp.UI.Mapping.Auto_Mapper
{
    public class UserCreateModelProfile : Profile
    {
        public UserCreateModelProfile()
        {
            CreateMap<AppUserCreateDto, UserCreateModel>()
    .ForMember(dest => dest.ConfirmPassword, opt => opt.Ignore()).ReverseMap();

        }
    }
}
