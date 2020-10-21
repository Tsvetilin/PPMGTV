using Data.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace Data.Common
{
    internal static class EntityRelationsConfiguration
    {
#pragma warning disable IDE0060 // Remove unused parameter
        public static void Configure(this ModelBuilder modelBuilder)
#pragma warning restore IDE0060 // Remove unused parameter
        {
            modelBuilder.Entity<ApplicationUser>()
                .HasMany(e => e.Roles)
                .WithOne()
                .HasForeignKey(e => e.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }

        public static void ConfigureUserIdentityRelations(this ModelBuilder builder, Type contextType)
        {
            builder.ApplyConfigurationsFromAssembly(contextType.Assembly);

            builder.Entity<ApplicationUser>()
                .HasMany(e => e.Claims)
                .WithOne()
                .HasForeignKey(e => e.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ApplicationUser>()
                .HasMany(e => e.Logins)
                .WithOne()
                .HasForeignKey(e => e.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ApplicationUser>()
                .HasMany(e => e.Roles)
                .WithOne()
                .HasForeignKey(e => e.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
       
    }
}
