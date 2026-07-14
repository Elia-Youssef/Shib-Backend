using Microsoft.EntityFrameworkCore;

public class User_ApplicationDbContext : DbContext
{
    public User_ApplicationDbContext(DbContextOptions<User_ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<User> USR_Users { get; set; }

    public DbSet<UserMachine> USR_Machine { get; set; }

    public DbSet<UserWallet> USR_Wallet { get; set; }

    public DbSet<UserNFT> USR_NFT { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // Configure entity properties if needed
        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.CreatedOn)
                .HasColumnType("timestamptz"); // PostgreSQL timestamp (without time zone)
        });
        modelBuilder.Entity<UserMachine>(entity =>
        {
            entity.Property(e => e.EffectiveDate)
                .HasColumnType("timestamptz"); // PostgreSQL timestamp (without time zone)
            entity.Property(e => e.ExpiryDate)
                .HasColumnType("timestamptz"); // PostgreSQL timestamp (without time zone)
        });
    }
}
