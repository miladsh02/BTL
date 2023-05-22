using BTL.Models;
using Data.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using Domain.Entity;

namespace BTL.Data
{
    public class ApplicationDbContext : IdentityDbContext<CustomIdentityUserModel>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<CartModel> Carts { get; set; }
        public DbSet<ProductModel> Products { get; set; }
        public DbSet<ProductTemplateModel> ProductsTemplate { get; set; }
        public DbSet<StudentModel> Students { get; set; }
        public DbSet<OrderModel> Order { get; set; }
        public DbSet<TransactionModel> Transaction { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            RenameIdentityTables(builder);
        }
        protected void RenameIdentityTables(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.HasDefaultSchema("CstUserMngt");
            builder.Entity<CustomIdentityUserModel>(entity =>
            {
                entity.ToTable(name: "Users");
            });
            builder.Entity<IdentityRole>(entity =>
            {
                entity.ToTable(name: "Roles");
            });
            builder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.ToTable("UserRoles");
            });
            builder.Entity<IdentityUserClaim<string>>(entity =>
            {
                entity.ToTable("UserClaims");
            });
            builder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.ToTable("UserLogins");
            });
            builder.Entity<IdentityRoleClaim<string>>(entity =>
            {
                entity.ToTable("RoleClaims");
            });
            builder.Entity<IdentityUserToken<string>>(entity =>
            {
                entity.ToTable("UserTokens");
            });

            builder.Entity<CustomIdentityUserModel>()
                .HasOne(a => a.Student)
                .WithOne(a => a.CustomIdentityUserModel)
                .HasForeignKey<StudentModel>(c => c.UserId);

        }
    }
}