using AdvertisementApp.Business.Extensions;
using AdvertisementApp.Business.Interfaces;
using AdvertisementApp.Common;
using AdvertisementApp.DataAccess.UnitOfWork;
using AdvertisementApp.Dtos.Interfaces;
using AdvertisementApp.Entities;
using AutoMapper;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdvertisementApp.Business.Services
{
    public class Service<CreateDto, UpdateDto, ListDto, T> : IService<CreateDto, UpdateDto, ListDto, T>
        where CreateDto : class, IDto, new()
        where UpdateDto : class, IUpdateDto, new()
        where ListDto : class, IDto, new()
        where T : BaseEntity
    {
        private readonly IValidator<CreateDto> _createValidator;
        private readonly IValidator<UpdateDto> _updateValidator;
        private readonly IMapper _mapper;
        private readonly IUow _uow;

        public Service(IValidator<CreateDto> createValidator, IValidator<UpdateDto> updateValidator, IMapper mapper, IUow uow)
        {
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _mapper = mapper;
            _uow = uow;
        }

        public async Task<IResponse<CreateDto>> CreateAsync(CreateDto dto)
        {
            var result = await _createValidator.ValidateAsync(dto);
            if (result.IsValid)
            {
                await _uow.GetRepository<T>().CreateAsync(_mapper.Map<T>(dto));
                await _uow.SaveChangesAsync();
                return new Response<CreateDto>(ResponseType.Success, dto);
            }
            return new Response<CreateDto>(dto, result.ConvertToList());
        }

        public async Task<IResponse<List<ListDto>>> GetAllAsync()
        {
            var list = await _uow.GetRepository<T>().GetAllAsync();
            var result = _mapper.Map<List<ListDto>>(list);
            return new Response<List<ListDto>>(ResponseType.Success, result);
        }

        public async Task<IResponse<IDto>> GetByIdAsync<IDto>(int id)
        {
            var data = await _uow.GetRepository<T>().GetByFilterAsync(x => x.Id == id);
            if (data == null)
                return new Response<IDto>(ResponseType.NotFound, $"{id} ye sahip data bulunamadi");
            var result = _mapper.Map<IDto>(data);
            return new Response<IDto>(ResponseType.Success, result);
        }
        public async Task<IResponse> RemoveAsync(int id)
        {
            var data = await _uow.GetRepository<T>().FindAsync(id);
            if (data == null)
                return new Response(ResponseType.NotFound, $"{id} ye sahip data bulunamadi");
            _uow.GetRepository<T>().Remove(data);
            await _uow.SaveChangesAsync();
            return new Response(ResponseType.Success);
        }

        public async Task<IResponse<UpdateDto>> UpdateAsync(UpdateDto dto)
        {
            var result = _updateValidator.Validate(dto);
            if (result.IsValid)
            {
                var unchangedData = await _uow.GetRepository<T>().FindAsync(dto.Id);
                if (unchangedData == null)
                    return new Response<UpdateDto>(ResponseType.NotFound, $"{dto.Id} id-e sahip data bulunamadi");
                var entity = _mapper.Map<T>(dto);
                _uow.GetRepository<T>().Update(entity, unchangedData);
                await _uow.SaveChangesAsync();
                return new Response<UpdateDto>(ResponseType.Success, dto);
            }
            return new Response<UpdateDto>(dto, result.ConvertToList());
        }
    }
}
