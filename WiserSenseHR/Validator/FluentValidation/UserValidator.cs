using FluentValidation;
using WiserSenseHR.Data.Entities;

namespace WiserSenseHR.Validator.FluentValidation
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(user => user.Name)
                .NotEmpty().WithMessage("İsim alanı boş olamaz")
                .MaximumLength(50).WithMessage("İsim en fazla 50 karakter olabilir");

            RuleFor(user => user.Email)
                .NotEmpty().WithMessage("E-posta alanı boş olamaz")
                .EmailAddress().WithMessage("Geçerli bir e-posta adresi giriniz")
                .EmailAddress().When(x => !string.IsNullOrWhiteSpace(x.Email));

            RuleFor(user => user.Password)
                .NotEmpty().WithMessage("Şifre boş olamaz")
                .MinimumLength(6).WithMessage("Şifre en az 6 karakter olmalıdır")
                .When(x => x.Id == Guid.Empty); ;

            RuleFor(user => user.Role)
                .NotNull().WithMessage("Rol seçimi zorunludur")
                .IsInEnum().WithMessage("Geçerli bir rol seçiniz");
               
            RuleFor(user => user.Department)
                .NotNull().WithMessage("Departman seçimi zorunludur")
                .IsInEnum().WithMessage("Geçerli bir departman seçiniz");
        }
    }
}
