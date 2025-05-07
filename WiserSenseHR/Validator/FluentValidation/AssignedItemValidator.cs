using FluentValidation;
using WiserSenseHR.Data.Entities;

namespace WiserSenseHR.Validator.FluentValidation
{
    public class AssignedItemValidator : AbstractValidator<AssignedItem>
    {
        public AssignedItemValidator()
        {
            RuleFor(assignedItem => assignedItem.ItemNames)
                .NotEmpty().WithMessage("En az bir eşya seçilmelidir.");
                


            RuleFor(assignedItem => assignedItem.Description)
                .MaximumLength(500).WithMessage("Açıklama en fazla 500 karakter olmalıdır.")
                .When(x => !string.IsNullOrEmpty(x.Description));

            RuleFor(assignedItem => assignedItem.UserId)
                .NotEmpty().WithMessage("Çalışan ID'si gereklidir.");
        }
    }
}
