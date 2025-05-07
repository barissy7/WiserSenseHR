using WiserSenseHR.Data.Enums;

namespace WiserSenseHR.Data.Entities;

public class User
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public bool Active { get; set; }
    public UserRole Role {  get; set; }
    public UserDepartment Department { get; set; }
    public virtual UserInfo? UserInfo { get; set; } = new();
    public ICollection<AssignedItem> AssignedItems { get; set; } = new List<AssignedItem>();
}



