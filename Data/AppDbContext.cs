using Microsoft.EntityFrameworkCore;
using TakeHomeAPI.Models;

namespace TakeHomeAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        // Existing User table (used for authentication)
        public DbSet<User> Users { get; set; }

        // New tables:
        public DbSet<Product> Products { get; set; }
        public DbSet<Packaging> Packagings { get; set; }
        public DbSet<Item> Items { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure Product
            modelBuilder.Entity<Product>()
                .HasKey(p => p.ProductID);
            modelBuilder.Entity<Product>()
                .Property(p => p.ProductName)
                .IsRequired()
                .HasMaxLength(100);
            modelBuilder.Entity<Product>()
                .Property(p => p.Description)
                .HasMaxLength(255);

            // Configure Packaging
            modelBuilder.Entity<Packaging>()
                .HasKey(p => p.PackagingID);
            modelBuilder.Entity<Packaging>()
                .Property(p => p.PackagingType)
                .IsRequired()
                .HasMaxLength(50);
            modelBuilder.Entity<Packaging>()
                .Property(p => p.PackagingName)
                .HasMaxLength(100);
            modelBuilder.Entity<Packaging>()
                .HasOne(p => p.Product)
                .WithMany(p => p.Packagings)
                .HasForeignKey(p => p.ProductID)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Packaging>()
                .HasOne(p => p.ParentPackaging)
                .WithMany(p => p.ChildPackagings)
                .HasForeignKey(p => p.ParentPackagingID)
                .OnDelete(DeleteBehavior.NoAction);

            // Configure Item
            modelBuilder.Entity<Item>()
                .HasKey(i => i.ItemID);
            modelBuilder.Entity<Item>()
                .Property(i => i.ItemName)
                .IsRequired()
                .HasMaxLength(100);
            modelBuilder.Entity<Item>()
                .HasOne(i => i.Packaging)
                .WithMany(p => p.Items)
                .HasForeignKey(i => i.PackagingID)
                .OnDelete(DeleteBehavior.Cascade);

            //indexing
            modelBuilder.Entity<Packaging>()
                .HasIndex(p => p.ProductID)
                .HasDatabaseName("IDX_Packaging_ProductID");

            modelBuilder.Entity<Packaging>()
                .HasIndex(p => p.ParentPackagingID)
                .HasDatabaseName("IDX_Packaging_ParentPackagingID");

            modelBuilder.Entity<Item>()
                .HasIndex(i => i.PackagingID)
                .HasDatabaseName("IDX_Item_PackagingID");

            base.OnModelCreating(modelBuilder);
        }
    }
}
