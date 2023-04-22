using Microsoft.EntityFrameworkCore;
using SimpD.Entity;

namespace SimpD;

public class MainContext : DbContext
{
    private const string Database = "data.db";
    private string databasePath;
    
    public DbSet<Container> Containers { get; set; }
    public DbSet<Mount> Mounts { get; set; }
    public DbSet<Port> Ports { get; set; }
    public DbSet<EnvironmentVariable> EnvironmentVariables { get; set; }

    public MainContext()
    {
        databasePath = Path.Join(Directory.GetCurrentDirectory(), Database);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={databasePath}");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Container>().HasIndex(c => c.Name).IsUnique();
    }
}
