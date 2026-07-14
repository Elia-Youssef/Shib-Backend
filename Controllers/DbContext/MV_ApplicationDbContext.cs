using Microsoft.EntityFrameworkCore;

public class MV_ApplicationDbContext : DbContext
{
    public MV_ApplicationDbContext(DbContextOptions<MV_ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<MVLands> MV_Lands { get; set; }
   // public DbSet<MVCoordinates> MV_Land_Coordinates { get; set; }
    public DbSet<MVBids> MV_Land_Bids { get; set; }
    public DbSet<MVMints> MV_Land_Mints { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // Configure entity properties if needed
        modelBuilder.Entity<MVBids>(entity =>
        {
            entity.Property(e => e.createdAt)
                .HasColumnType("timestamptz"); // PostgreSQL timestamp (without time zone)
        });
        modelBuilder.Entity<MVMints>(entity =>
        {
            entity.Property(e => e.createdAt)
                .HasColumnType("timestamptz"); // PostgreSQL timestamp (without time zone)
        });
    }
}

