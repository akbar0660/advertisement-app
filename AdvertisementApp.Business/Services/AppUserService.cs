using AdvertisementApp.Business.Extensions;
using AdvertisementApp.Business.Interfaces;
using AdvertisementApp.Common;
using AdvertisementApp.DataAccess.UnitOfWork;
using AdvertisementApp.Dtos;
using AdvertisementApp.Entities;
using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdvertisementApp.Business.Services
{
    public class AppUserService : Service<AppUserCreateDto, AppUserUpdateDto, AppUserListDto, AppUser>, IAppUserService
    {
        private readonly IUow _uow;
        private readonly IMapper _mapper;
        private readonly IValidator<AppUserCreateDto> _validator;
        private readonly IValidator<AppUserLoginDto> _loginValidator;
        public AppUserService(IValidator<AppUserCreateDto> createValidator, IValidator<AppUserUpdateDto> updateValidator, IMapper mapper, IUow uow, IValidator<AppUserCreateDto> validator, IValidator<AppUserLoginDto> loginValidator) : base(createValidator, updateValidator, mapper, uow)
        {
            _uow = uow;
            _mapper = mapper;
            _validator = validator;
            _loginValidator = loginValidator;
        }

        public async Task<IResponse<AppUserCreateDto>> CreateWithRoleAsync(AppUserCreateDto dto, int roleId)
        {
            var validationResult = _validator.Validate(dto);
            if (validationResult.IsValid)
            {
                var user = _mapper.Map<AppUser>(dto);
                await _uow.GetRepository<AppUser>().CreateAsync(user);
                await _uow.GetRepository<AppUserRole>().CreateAsync(new AppUserRole
                {
                    AppUser = user,
                    AppRoleId = roleId
                });
                await _uow.SaveChangesAsync();
            }
            return new Response<AppUserCreateDto>(dto, validationResult.ConvertToList());
        }

        public async Task<IResponse<AppUserListDto>> CheckUserAsync(AppUserLoginDto dto)
        {
            var validator = _loginValidator.Validate(dto);
            if (validator.IsValid)
            {
                var user = await _uow.GetRepository<AppUser>().GetByFilterAsync(x => x.Username == dto.Username && x.Password == dto.Password);
                if (user != null)
                {
                    var listDto = _mapper.Map<AppUserListDto>(user);
                    return new Response<AppUserListDto>(ResponseType.Success, listDto);
                }
                return new Response<AppUserListDto>(ResponseType.NotFound, "Username or password is incorrect");
            }
            return new Response<AppUserListDto>(ResponseType.ValidationError, "Username and password can not be empty");
        }

        public async Task<IResponse<List<AppRoleListDto>>> GetRoleByUserIdAsync(int userId)
        {
            List<AppRole> roles = await _uow.GetRepository<AppRole>().GetAllAsync(x => x.AppUserRoles.Any(x => x.Id == userId));
            if (roles == null)
            {
                return new Response<List<AppRoleListDto>>(ResponseType.NotFound, "Roles can not be find");
            }
            List<AppRoleListDto> dto = _mapper.Map<List<AppRoleListDto>>(roles);

            return new Response<List<AppRoleListDto>>(ResponseType.Success, dto);
        }
    }
}
