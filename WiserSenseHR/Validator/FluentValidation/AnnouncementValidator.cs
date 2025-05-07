using FluentValidation;
using WiserSenseHR.Data.Entities;

namespace WiserSenseHR.Validator.FluentValidation
{
    public class AnnouncementValidator : AbstractValidator<Announcement>
    {
        public AnnouncementValidator()
        {
            RuleFor(announcement => announcement.Name)
                .NotEmpty().WithMessage("Duyuru adı boş olamaz")
                .MaximumLength(100).WithMessage("Duyuru adı en fazla 100 karakter olabilir");

            RuleFor(announcement => announcement.Description)
                .MaximumLength(200).WithMessage("Açıklama en fazla 200 karakter olabilir")
                .NotEmpty().WithMessage("Duyuru açıklaması boş olamaz");

            RuleFor(announcement => announcement.AnnouncementDate)
                .NotEmpty().WithMessage("Tarih alanı boş olamaz")
                .GreaterThan(DateTime.Now).WithMessage("Duyuru tarihi geçmiş bir tarih olamaz");
        }
    }
}
