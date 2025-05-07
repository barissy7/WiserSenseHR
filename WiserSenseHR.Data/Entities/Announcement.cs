namespace WiserSenseHR.Data.Entities
{
    public class Announcement
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public DateTime AnnouncementDate { get; set; }
        public string? Description { get; set; }


    }
}
