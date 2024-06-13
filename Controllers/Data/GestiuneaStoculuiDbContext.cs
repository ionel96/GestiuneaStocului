using GestiuneaStocului.Models;
using Microsoft.EntityFrameworkCore;

public class GestiuneaStoculuiDbContext : DbContext {

    public GestiuneaStoculuiDbContext(DbContextOptions<GestiuneaStoculuiDbContext> options) : base(options) {
    }

    public DbSet<Products> Products { get; set; }

    public DbSet<ProductReports> ProductReports { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.Entity<Products>().HasMany(p => p.ProductReports).WithOne(pr => pr.Product).HasForeignKey(pr => pr.ProductId).IsRequired();
        modelBuilder.Entity<ProductReports>().ToTable("ProductReports");
        modelBuilder.Entity<ProductReports>().HasKey(pr => pr.ReportId);
        modelBuilder.Entity<Products>().ToTable("Products");
        modelBuilder.Entity<Products>().HasKey(p => p.ProductId);
    }
}