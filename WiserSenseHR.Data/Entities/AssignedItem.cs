using WiserSenseHR.Data.Enums;

namespace WiserSenseHR.Data.Entities;

public class AssignedItem
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public List<AssignedItemName> ItemNames { get; set; } = new List<AssignedItemName>();
    public string? Description { get; set; } 
    public Guid UserId { get; set; }
    public User User { get; set; } 
}

