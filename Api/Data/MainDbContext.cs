
using Microsoft.EntityFrameworkCore;

public class MainDbContext : DbContext
{
    public MainDbContext(DbContextOptions<MainDbContext> options) : base(options)
    {
    }

    //DbSets
    public DbSet<House> Houses => Set<House>(); // this is readonly.
    public DbSet<Bid> Bids => Set<Bid>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Configures Entity Framework Core to use a SQLite database located in the local application data folder.
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);

        optionsBuilder
            .UseSqlite($"Data Source={Path.Join(path, "housesCourse.db")}");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        SeedData.Seed(modelBuilder);
    }
}