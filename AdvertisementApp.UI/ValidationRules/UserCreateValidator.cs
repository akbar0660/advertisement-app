using AdvertisementApp.UI.Models;
using FluentValidation;
using System;

namespace AdvertisementApp.UI.ValidationRules
{
    public class UserCreateValidator : AbstractValidator<UserCreateModel>
    {
        public UserCreateValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty();
            RuleFor(x => x.Username).NotEmpty();
            RuleFor(x => x.Surname).NotEmpty();
            RuleFor(x => x.PhoneNumber).NotEmpty();
            RuleFor(x => x.GenderId).NotEmpty();
            RuleFor(x => x.Password).NotEmpty();
            RuleFor(x => x.ConfirmPassword).NotEmpty();
            RuleFor(x => x.Password).MinimumLength(3);
            RuleFor(x => x.ConfirmPassword).Equal(x => x.Password).WithMessage("Passwords don't match");
            RuleFor(x => x.Username).MinimumLength(3);
            RuleFor(x => new
            {
                x.Username,
                x.FirstName
            }).Must(x => CanNotEqualFirstname(x.Username, x.FirstName)).WithMessage("Username cannot be firstname").When(x => x.Username != null && x.FirstName != null);
        }

        private bool CanNotEqualFirstname(string username, string firstname)
        {
            return !username.Contains(firstname);
        }
    }
}
