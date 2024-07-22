using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VendingMachine.Models;

namespace VendingMachine.Data
{
    public class VendingMachineContext : IdentityDbContext<IdentityUser>
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        public VendingMachineContext (DbContextOptions<VendingMachineContext> options)
            : base(options)
        {
        }

        public DbSet<VendingMachine.Models.Product> Product { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);
            // Additional model configurations if needed

            base.OnModelCreating(modelBuilder);

            // This means Entity Framework will provide a default precision to the database.
            // The default is typically (18, 2). That means it will store 18 total digits, with 2 of those digits being to the right of the decimal point.
            // If your record has more than 2 decimal points, SQL Server will truncate the extras.
            // If your record has more than 18 total digits, you will get an "out of range" error.
            // The easiest fix is to use Data Annotations to declare a default on your model.
            // The data annotation looks like: [Column(TypeName = "decimal(18,2)")]
            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18,3)");

            modelBuilder.Entity<Login>().HasNoKey();
            modelBuilder.Entity<ErrorViewModel>().HasNoKey();
            modelBuilder.Entity<Product>().Ignore(p => p.PurchaseQuantity);
            modelBuilder.Entity<ProductListViewModel>().Ignore(p => p.Id);
            modelBuilder.Entity<ProductListViewModel>().HasNoKey();

        }
        public DbSet<VendingMachine.Models.Account> Account { get; set; } = default!;
        public DbSet<VendingMachine.Models.Login> Login { get; set; } = default!;
    }
}
