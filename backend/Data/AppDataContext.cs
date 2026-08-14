using backend.model;
using backend.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata; // Add this for the Strategy enum

namespace backend.data;

public class AppDataContext : IdentityDbContext<User, IdentityRole<int>, int>
{
    public AppDataContext(DbContextOptions<AppDataContext> options) : base(options)
    {
    }

    public AppDataContext() : base()
    {
    }

    public override DbSet<User> Users { get; set; }
    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("Users");

            entity.Property(u => u.Id)
                  .HasColumnName("UserId");

            entity.Property(u => u.Id)
                  .UseIdentityByDefaultColumn(); // PostgreSQL strategy

            entity.Property(u => u.UserRole)
                .HasConversion<string>() // Store enum as string
                    .IsRequired()
                    .HasMaxLength(50);

            entity.Property(u => u.UserName)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(x => x.PasswordHash)
                .IsRequired(false);

            entity.HasIndex(x => x.UserName)
                .IsUnique();

            entity.OwnsOne(u => u.AvatarUri, avatar =>
            {
                avatar.Property(a => a.Small)
                    .HasMaxLength(200)
                    .HasColumnName("AvatarSmall");

                avatar.Property(a => a.Normal)
                    .HasMaxLength(200)
                    .HasColumnName("AvatarNormal");

                avatar.Property(a => a.Large)
                    .HasMaxLength(200)
                    .HasColumnName("AvatarLarge");

                avatar.Ignore(a => a.Id);
            });

            entity.HasMany(u => u.Products)
                .WithOne(p => p.User)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Products");

            entity.Property(u => u.Id)
            .HasColumnName("ProductId");

            entity.Property(u => u.Id)
                  .UseIdentityByDefaultColumn(); // PostgreSQL strategy

            entity.Property(u => u.ProductName)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(u => u.ProductBrand)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(u => u.ProductCategory)
            .HasConversion<string>() // Store enum as string
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(u => u.ProductUnit)
            .HasConversion<string>() // Store enum as string
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(u => u.ProductQuantity)
                .IsRequired();

            entity.Property(u => u.ProductPrice)
                .IsRequired()
                .HasColumnType("decimal(18,2)"); // Adjust precision and scale as needed

            entity.Property(u => u.ProductStatus)
            .HasConversion<string>() // Store enum as string
                .HasMaxLength(50);
        });
    }



}
