using ApiApp.Data.Config;
using Internship.Model;
using Microsoft.EntityFrameworkCore;

namespace ApiApp.Data;

public class APIDbContext : DbContext
{
    public DbSet<Person> Persons { get; set; }
    public DbSet<Position> Positions { get; set; }
    public DbSet<Salary> Salaries { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<PersonDetails> PersonDetails { get; set; }
    public string DbPath { get; }

    public APIDbContext()
    {
        var path = "C:\\Users\\cosmin\\Documents\\GitHub\\TEC-Internship-Cosmin-Fuica\\TEC-Internship-main\\Database";
        DbPath = Path.Join(path, "Internship.db");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    => options.UseSqlite($"Data Source={DbPath}");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new RoleConfiguration());
        modelBuilder.ApplyConfiguration(new UserRoleConfiguration());
        modelBuilder.ApplyConfiguration(new PersonDetailsConfiguration());
    }
}