using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using BookRental.Entities;


namespace BookRental.Persistence
{
    public class BookDbContext:IdentityDbContext<UserEntity, UserRoleEntity, string>
    {
        public DbSet<BookEntity> Books { get; set; } // Add the book table to the database
        


        public BookDbContext(DbContextOptions<BookDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UserEntity>(entity =>
            {
                entity.ToTable("Users");
            });

            builder.Entity<UserRoleEntity>(entity =>
            {
                entity.ToTable("Roles");
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

            builder.Entity<BookEntity>(entity =>
            {
                entity.ToTable("Books");
            });
        }

    }
}
