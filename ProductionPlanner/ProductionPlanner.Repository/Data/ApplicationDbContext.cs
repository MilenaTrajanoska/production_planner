using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProductionPlanner.Domain.Models;
using System;

namespace ProductionPlanner.Repository.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Company> Companies { get; set; }
        public virtual DbSet<ProductHistory> ProductHistories { get; set; }
        public virtual DbSet<Material> Materials { get; set; }
        public virtual DbSet<MaterialForProduct> MaterialForProduct { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            Company company = Company.getInstance();
            company.Id = 1;
            company.NumberOfWS = 0;
            company.WSCapacity = 4;

            builder.Entity<Product>()
                .HasAlternateKey(p => p.ProductName);
            builder.Entity<Order>()
                .HasAlternateKey(o => o.OrderName);
            builder.Entity<Material>()
                .HasAlternateKey(m => m.MaterialName);

            builder.Entity<Company>().HasData(company);

            builder.Entity<MaterialForProduct>()
              .HasOne(z => z.Product)
              .WithMany(z => z.MaterialForProduct)
              .HasForeignKey(z => z.ProductId)
              .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<MaterialForProduct>()
                .HasOne(z => z.Material)
                .WithMany(z => z.MaterialForProduct)
                .HasForeignKey(z => z.MaterialId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
