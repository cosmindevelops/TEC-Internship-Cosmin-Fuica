namespace ApiApp.Model;

public class Role
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}