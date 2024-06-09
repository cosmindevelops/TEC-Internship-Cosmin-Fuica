using ApiApp.Data.Config;
using Internship.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ApiApp.Data;

public class APIDbContext : DbContext
{
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly string _dbPath;

    public DbSet<Person> Persons { get; set; }
    public DbSet<Position> Positions { get; set; }
    public DbSet<Salary> Salaries { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<PersonDetails> PersonDetails { get; set; }
    public string DbPath { get; }

    public APIDbContext(DbContextOptions<APIDbContext> options, IPasswordHasher<User> passwordHasher, IConfiguration configuration)
        : base(options)
    {
        _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
        _dbPath = configuration.GetValue<string>("DatabasePath");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        if (!string.IsNullOrEmpty(_dbPath))
        {
            options.UseSqlite($"Data Source={_dbPath}");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new RoleConfiguration());
        modelBuilder.ApplyConfiguration(new UserRoleConfiguration());
        modelBuilder.ApplyConfiguration(new PersonDetailsConfiguration());
    }

    public static void SeedData(APIDbContext context)
    {
        Console.WriteLine("Starting database seeding...");

        // Seed roles
        var roles = new List<Role>
        {
            new Role { Id = Guid.NewGuid(), Name = "Admin" },
            new Role { Id = Guid.NewGuid(), Name = "User" }
        };

        foreach (var role in roles)
        {
            if (!context.Roles.Any(r => r.Name == role.Name))
            {
                context.Roles.Add(role);
            }
        }

        context.SaveChanges();

        var adminRole = context.Roles.FirstOrDefault(r => r.Name == "Admin");
        if (adminRole == null)
        {
            return;
        }

        // Seed admin user
        if (!context.Users.Any(u => u.Email == "admin@gmail.com"))
        {
            var adminUser = new User
            {
                Id = Guid.NewGuid(),
                Username = "admin",
                Email = "admin@gmail.com",
                PasswordHash = context._passwordHasher.HashPassword(null, "password123")
            };

            context.Users.Add(adminUser);

            var userRole = new UserRole
            {
                UserId = adminUser.Id,
                RoleId = adminRole.Id
            };

            context.UserRoles.Add(userRole);
        }

        context.SaveChanges();
    }
}