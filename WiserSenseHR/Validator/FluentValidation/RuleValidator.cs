using WiserSenseHR.Data.Entities;
using FluentValidation;


namespace WiserSenseHR.Validator.FluentValidation
{
    public class RuleValidator : AbstractValidator<Rule>
    {
        public RuleValidator()
        {
            RuleFor(rule => rule.Description)
                .NotEmpty().WithMessage("Açıklama boş olamaz.")
                .Length(5, 2000).WithMessage("Açıklama 5 ile 2000 karakter arasında olmalıdır.");
        }
    }
}