namespace ApiApp.Model;

public class User
{
    public Guid Id { get; set; }
    public string Username { get; set; }
    public string PasswordHash { get; set; }
    public string Email { get; set; }
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}