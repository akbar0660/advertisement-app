using AdvertisementApp.Common.Enums;
using AdvertisementApp.Dtos;
using FluentValidation;

namespace AdvertisementApp.Business.ValidationRules
{
    public class AdvertisementAppUserCreateDtoValidator : AbstractValidator<AdvertisementAppUserCreateDto>
    {
        public AdvertisementAppUserCreateDtoValidator()
        {
            RuleFor(x => x.AdvertisementAppUserStatusId).NotEmpty();
            RuleFor(x => x.AdvertisementId).NotEmpty();
            RuleFor(x => x.MilitaryStatusId).NotEmpty();
            RuleFor(x => x.CvPath).NotEmpty();
            RuleFor(x => x.EndDate).NotEmpty().When(x=>x.MilitaryStatusId==(int)MilitaryStatusType.Tecilli);
        }
    }
}
