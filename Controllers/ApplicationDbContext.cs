using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<System_NFT_Item> NftItem { get; set; }
    public DbSet<System_NFT_Attribute> NftAttribute { get; set; }
    public DbSet<System_NFT_Metadata> NftMetadata { get; set; }
    public DbSet<System_NFT_Token> NftToken { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // Configure entity properties if needed
    }
}
