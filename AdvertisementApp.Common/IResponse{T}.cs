using System.Collections.Generic;

namespace AdvertisementApp.Common
{
    public interface IResponse<T> : IResponse
    {
         T Data { get; set; }
         List<CustomValidationError> Errors { get; set; }
    }
}
