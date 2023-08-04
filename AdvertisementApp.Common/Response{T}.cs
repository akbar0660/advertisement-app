using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvertisementApp.Common
{
    public class Response<T> : Response , IResponse<T>
    {
        public T Data { get; set; }
        
        public List<CustomValidationError> Errors { get; set; }
        public Response(ResponseType responseType , string message):base(responseType,message)
        {
            
        }
        public Response(ResponseType responseType , T data) : base(responseType)
        {
            Data = data;
        }
        public Response(ResponseType responseType,T data,List<CustomValidationError> errors):base(responseType)
        {
            Data = data;
            Errors = errors;
        }
        public Response(T data , List<CustomValidationError> errors)
        {
            Data = data;
            Errors = errors;
        }
        public Response(ResponseType responseType) : base(responseType)
        {
            
        }

    }
}
