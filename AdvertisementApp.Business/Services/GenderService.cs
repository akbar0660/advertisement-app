using AdvertisementApp.Business.Interfaces;
using AdvertisementApp.DataAccess.UnitOfWork;
using AdvertisementApp.Dtos;
using AdvertisementApp.Entities;
using AutoMapper;
using FluentValidation;

namespace AdvertisementApp.Business.Services
{
    partial class GenderService : Service<GenderCreateDto, GenderUpdateDto, GenderListDto, Gender>, IGenderService
    {
        public GenderService(IValidator<GenderCreateDto> createValidator, IValidator<GenderUpdateDto> updateValidator, IMapper mapper, IUow uow) : base(createValidator, updateValidator, mapper, uow)
        {
        }
    }
}
