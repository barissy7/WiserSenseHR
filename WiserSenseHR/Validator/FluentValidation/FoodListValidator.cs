using FluentValidation;
using WiserSenseHR.Data.Entities;

namespace WiserSenseHR.Validator.FluentValidation
{
    public class FoodListValidator : AbstractValidator<FoodList>
    {
        public FoodListValidator()
        {
            //RuleFor(food => food.ImageUrl)
            //    .NotEmpty().WithMessage("Fotoğraf alanı boş olamaz")
            //    .MaximumLength(500).WithMessage("Fotoğraf URL'si en fazla 500 karakter olabilir");

            RuleFor(food => food.Description)
                .MaximumLength(200).WithMessage("Açıklama en fazla 200 karakter olabilir")
                .When(food => !string.IsNullOrEmpty(food.Description));

        }
    }
}
