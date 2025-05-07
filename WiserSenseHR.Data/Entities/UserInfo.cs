namespace WiserSenseHR.Data.Entities
{
    public class UserInfo
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }
        public DateTime? Birthday { get; set; }
        public DateTime? JobStartDate { get; set; }
        public long? PhoneNumber { get; set; }
        public string? JobTitle { get; set; }
        public virtual User? User { get; set; }
    }
}
