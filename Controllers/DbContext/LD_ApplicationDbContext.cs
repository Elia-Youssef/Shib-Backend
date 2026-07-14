using Microsoft.EntityFrameworkCore;

public class LD_ApplicationDbContext : DbContext
{
    public LD_ApplicationDbContext(DbContextOptions<LD_ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<LDPlayer> LD_Player { get; set; }
    public DbSet<LDPlayerStats> LD_PlayerStats { get; set; }
    public DbSet<LDGameSession> LD_GameSession { get; set; }
    public DbSet<LDPlayerSession> LD_PlayerSession { get; set; }
    public DbSet<LDMaps> LD_Maps { get; set; }
    public DbSet<LDPlayerMapRecord> LD_PlayerMapRecord { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // Configure entity properties if needed
        modelBuilder.Entity<LDGameSession>(entity =>
        {
            entity.Property(e => e.Created_At)
                .HasColumnType("timestamptz"); // PostgreSQL timestamp (without time zone)
        });
    }
}

