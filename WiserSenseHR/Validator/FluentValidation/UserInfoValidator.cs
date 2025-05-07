using FluentValidation;
using WiserSenseHR.Data.Entities;

namespace WiserSenseHR.Validator.FluentValidation
{
    public class UserInfoValidator : AbstractValidator<UserInfo>
    {
        public UserInfoValidator()
        {
            RuleFor(userInfo => userInfo.PhoneNumber)
                .GreaterThan(0).WithMessage("Geçerli bir telefon numarası girin")
                .Must(phone => phone.ToString().Length >= 10 && phone.ToString().Length <= 15)
                .WithMessage("Telefon numarası 10 ile 15 haneli olmalıdır");

            RuleFor(userInfo => userInfo.Birthday)
                .LessThan(DateTime.Now).WithMessage("Doğum tarihi gelecekte olamaz");

            RuleFor(userInfo => userInfo.JobStartDate)
                .LessThanOrEqualTo(DateTime.Now.AddMonths(6)).WithMessage("İşe başlama tarihi 6 aydan fazla bir süre sonrasına olamaz.");

            RuleFor(userInfo => userInfo.JobTitle)
                .Length(3, 100).WithMessage("İş unvanı en az 3, en fazla 100 karakter olmalıdır");
        }
    }
}
