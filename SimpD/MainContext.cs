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
    public DbSet<Status> Statuses { get; set; }

    public MainContext()
    {
        databasePath = Path.Join(Directory.GetCurrentDirectory(), Database);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={databasePath}");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Container>().HasIndex(c => c.Name).IsUnique();
        
        modelBuilder.Entity<Container>().HasMany(c => c.Mounts)
            .WithOne(m => m.Owner).OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<Container>().HasMany(c => c.EnvironmentVariables)
            .WithOne(e => e.Owner).OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Container>().HasMany(c => c.Ports)
            .WithOne(p => p.Owner).OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Container>()
            .HasOne(c => c.Status)
            .WithOne(s => s.Container)
            .OnDelete(DeleteBehavior.Cascade)
            .HasForeignKey<Status>();
    }
}
