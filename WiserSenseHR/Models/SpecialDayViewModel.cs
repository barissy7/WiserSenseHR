namespace WiserSenseHR.Models
{
    public class SpecialDayViewModel
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public DateTime? Birthday { get; set; }
        public int? DaysUntilBirthday { get; set; }
        public DateTime? JobStartDate { get; set; }
        public int? DaysUntilJobStart { get; set; }
    }
}
