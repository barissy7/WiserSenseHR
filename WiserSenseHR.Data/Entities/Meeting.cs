namespace WiserSenseHR.Data.Entities
{
    public class Meeting
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime MeetingDate { get; set; }
        public DateTime EndDate { get; set; }

    }
}
