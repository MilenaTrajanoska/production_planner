using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProductionPlanner.Domain.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

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
            company.NumberOfWS = 12;
            company.WSCapacity = 24;
            company.isValid = true;
            company.IsSet = true;

            //builder.Entity<Product>()
            //    .HasAlternateKey(p => p.ProductName);
            //builder.Entity<Order>()
            //    .HasAlternateKey(o => o.OrderName);
            //builder.Entity<Material>()
            //    .HasAlternateKey(m => m.MaterialName);

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

            //builder.Entity<Product>()
            //.AfterUpdate(trigger => trigger.Action(action => action
            //.Condition((productBeforeUpdate, productAfterUpdate) => productBeforeUpdate.IsValid)
            //.Update<ProductHistory>(
            //    (productBeforeUpdate, productAfterUpdate, oldHistory) => oldHistory.ProductId == productBeforeUpdate.Id && oldHistory.Version == productBeforeUpdate.CurrentVersion,
            //    (productBeforeUpdate, productAfterUpdate, oldHistory) => new ProductHistory { ModificationDate = DateTime.Now, isValid = false}
            //    )
            // .Insert<ProductHistory>((productBeforeUpdate, productAfterUpdate) => new ProductHistory
            //{
            //    ProductId = productAfterUpdate.Id,
            //    ProductName = productAfterUpdate.ProductName,
            //    Version = productBeforeUpdate.CurrentVersion + 1,
            //    PostProcessingWaitTime = productAfterUpdate.PostProcessingWaitTime,
            //    PreProcessingWaitTime = productAfterUpdate.PreProcessingWaitTime,
            //    InProcessTime = productAfterUpdate.InProcessTime,
            //    SetUpTime = productAfterUpdate.SetUpTime,
            //    SellingPrice = productAfterUpdate.SellingPrice,
            //    TotalMaterialCost = productAfterUpdate.TotalMaterialCost(),
            //    WagePerHour = productAfterUpdate.WagePerHour,
            //    VariableOHPerDLHour = productAfterUpdate.VariableOHPerDLHour,
            //    InterestRate = productAfterUpdate.InterestRate,
            //    FixedOHPerYear = productAfterUpdate.FixedOHPerYear,
            //    CreationDate = DateTime.Now,
            //    ModificationDate = DateTime.Now,
            //    isValid = true
            //})
            //.Upsert<ProductHistory>(
            //    (productBeforeUpdate, productAfterUpdate) => new ProductHistory { ProductId = productBeforeUpdate.Id, Version = productBeforeUpdate.CurrentVersion },
            //    (productBeforeUpdate, productAfterUpdate) => new ProductHistory {
            //        ProductId = productBeforeUpdate.Id,
            //        ProductName = productBeforeUpdate.ProductName,
            //        Version = productBeforeUpdate.CurrentVersion,
            //        PostProcessingWaitTime = productBeforeUpdate.PostProcessingWaitTime,
            //        PreProcessingWaitTime = productBeforeUpdate.PreProcessingWaitTime,
            //        InProcessTime = productBeforeUpdate.InProcessTime,
            //        SetUpTime = productBeforeUpdate.SetUpTime,
            //        SellingPrice = productBeforeUpdate.SellingPrice,
            //        TotalMaterialCost = productBeforeUpdate.TotalMaterialCost(),
            //        WagePerHour = productBeforeUpdate.WagePerHour,
            //        VariableOHPerDLHour = productBeforeUpdate.VariableOHPerDLHour,
            //        InterestRate = productBeforeUpdate.InterestRate,
            //        FixedOHPerYear = productBeforeUpdate.FixedOHPerYear,
            //        CreationDate = DateTime.Now,
            //        ModificationDate = DateTime.Now,
            //        isValid = true},
            //    (productBeforeUpdate, productAfterUpdate, oldProductHistory) => new ProductHistory {
            //         Version = oldProductHistory.Version + 1,

            //    }
            //    )
            //));

            //builder.Entity<Product>()
            //.AfterInsert(trigger => trigger.Action(action => action
            // .Insert<ProductHistory>((insertedProduct) => new ProductHistory
            // {
            //     ProductId = insertedProduct.Id,
            //     ProductName = insertedProduct.ProductName,
            //     Version = insertedProduct.CurrentVersion,
            //     PostProcessingWaitTime = insertedProduct.PostProcessingWaitTime,
            //     PreProcessingWaitTime = insertedProduct.PreProcessingWaitTime,
            //     InProcessTime = insertedProduct.InProcessTime,
            //     SetUpTime = insertedProduct.SetUpTime,
            //     SellingPrice = insertedProduct.SellingPrice,
            //     TotalMaterialCost = insertedProduct.TotalMaterialCost(),
            //     WagePerHour = insertedProduct.WagePerHour,
            //     VariableOHPerDLHour = insertedProduct.VariableOHPerDLHour,
            //     InterestRate = insertedProduct.InterestRate,
            //     FixedOHPerYear = insertedProduct.FixedOHPerYear,
            //     CreationDate = DateTime.Now,
            //     ModificationDate = DateTime.Now,
            //     isValid = true
            // })));
        }
    }
