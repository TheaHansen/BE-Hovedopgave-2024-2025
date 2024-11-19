using Microsoft.EntityFrameworkCore;

namespace BE_Hovedopgave_2024_2025.Model;

public class OdontologicDbContext : DbContext
{
    public OdontologicDbContext() {}

    public OdontologicDbContext(DbContextOptions<OdontologicDbContext> options) : base(options) {}

    public DbSet<Product> Products { get; set; }
    public DbSet<Label> Labels { get; set; }
    public DbSet<Colour> Colours { get; set; }
    public DbSet<Size> Sizes { get; set; }
    public DbSet<Stock> Stocks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configuring the many-to-many relationship between Product and Label with custom column names
        modelBuilder.Entity<Product>()
            .HasMany(p => p.Labels) 
            .WithMany(l => l.Products) 
            .UsingEntity<Dictionary<string, object>>( 
                "LabelProduct",
                j => j.HasOne<Label>()
                    .WithMany()
                    .HasForeignKey("LabelId")
                    .HasConstraintName("FK_LabelProduct_Label_LabelId"), 
                j => j.HasOne<Product>() 
                    .WithMany() 
                    .HasForeignKey("ProductId")
                    .HasConstraintName("FK_LabelProduct_Product_ProductId")
            );
    }
    
}