
using AdvertisementApp.Business.Extensions;
using AdvertisementApp.Business.Interfaces;
using AdvertisementApp.Business.ValidationRules;
using AdvertisementApp.Common;
using AdvertisementApp.Common.Enums;
using AdvertisementApp.DataAccess.UnitOfWork;
using AdvertisementApp.Dtos;
using AdvertisementApp.Entities;
using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdvertisementApp.Business.Services
{
    public class AdvertisementAppUserService : IAdvertisementAppUserService
    {
        private readonly IUow _uow;
        private readonly IValidator<AdvertisementAppUserCreateDto> _createValidator;
        private readonly IMapper _mapper;

        public AdvertisementAppUserService(IUow uow, IValidator<AdvertisementAppUserCreateDto> createValidator, IMapper mapper)
        {
            _uow = uow;
            _createValidator = createValidator;
            _mapper = mapper;
        }

        public async Task<IResponse<AdvertisementAppUserCreateDto>> CreateAsync(AdvertisementAppUserCreateDto dto)
        {
            var result = _createValidator.Validate(dto);
            if (result.IsValid)
            {
                var control = await _uow.GetRepository<AdvertisementAppUser>().GetByFilterAsync(x => x.AppUserId == dto.AppUserId && x.AdvertisementId == dto.AdvertisementId);
                if (control == null)
                {
                    var entity = _mapper.Map<AdvertisementAppUser>(dto);
                    await _uow.GetRepository<AdvertisementAppUser>().CreateAsync(entity);
                    await _uow.SaveChangesAsync();
                    return new Response<AdvertisementAppUserCreateDto>(ResponseType.Success, dto);
                }
                else
                {
                    List<CustomValidationError> errors = new List<CustomValidationError> { new CustomValidationError
                {
                    ErrorMessage = "You already applied to this job",
                    PropertyName = ""
                } };
                    return new Response<AdvertisementAppUserCreateDto>(ResponseType.ValidationError, dto, errors);
                }
            }
            return new Response<AdvertisementAppUserCreateDto>(ResponseType.ValidationError, dto, result.ConvertToList());
        }

        public async Task<List<AdvertisementAppUserListDto>> GetListAsync(AdvertisementAppUserStatusType type)
        {
            var query = _uow.GetRepository<AdvertisementAppUser>().GetQuery();
            var list = await query.Include(x => x.Advertisement).Include(x => x.AdvertisementAppUserStatus).Include(x => x.MilitaryStatus).Include(x => x.AppUser).
                Where(x => x.AdvertisementAppUserStatusId == (int)type).ToListAsync();
            var dto = _mapper.Map<List<AdvertisementAppUserListDto>>(list);
            return dto;
        }

        public async Task SetStatusAsync(int advertisementAppUserId, AdvertisementAppUserStatusType type)
        {
            var advertisementAppUser = await _uow.GetRepository<AdvertisementAppUser>().FindAsync(advertisementAppUserId);
            advertisementAppUser.AdvertisementAppUserStatusId = (int)type;
            await _uow.SaveChangesAsync();
        }

    }
}
