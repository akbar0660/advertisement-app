using AdvertisementApp.Common;
using FluentValidation.Results;
using System.Collections.Generic;

namespace AdvertisementApp.Business.Extensions
{
    public static class CustomValidationErrorsExtension
    {
        public static List<CustomValidationError> ConvertToList(this ValidationResult result)
        {
            List<CustomValidationError> errors = new List<CustomValidationError>();
            foreach (var error in result.Errors)
            {
                errors.Add(new()
                {
                    ErrorMessage = error.ErrorMessage,
                    PropertyName = error.PropertyName
                });
            }
            return errors;
        }
    }
}
