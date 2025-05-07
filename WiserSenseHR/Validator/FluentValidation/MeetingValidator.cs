using FluentValidation;
using WiserSenseHR.Data.Entities;

namespace WiserSenseHR.Validator.FluentValidation
{
    public class MeetingValidator : AbstractValidator<Meeting>
    {
        public MeetingValidator()
        {
            RuleFor(meeting => meeting.Name)
                .NotEmpty().WithMessage("Toplantı adı boş olamaz")
                .MaximumLength(100).WithMessage("Toplantı adı en fazla 100 karakter olabilir");

            RuleFor(meeting => meeting.Description)
                .MaximumLength(500).WithMessage("Açıklama en fazla 500 karakter olabilir");

            RuleFor(meeting => meeting.MeetingDate)
                .NotEmpty().WithMessage("Tarih alanı boş olamaz")
                .GreaterThan(DateTime.Now).WithMessage("Toplantı tarihi geçmiş bir tarih olamaz");

            RuleFor(m => m.EndDate)
                .NotEmpty().WithMessage("Bitiş tarihi gereklidir.")
                .GreaterThan(m => m.MeetingDate).WithMessage("Bitiş tarihi, toplantı başlangıç tarihinden sonra olmalıdır.");
        }
    }
}

