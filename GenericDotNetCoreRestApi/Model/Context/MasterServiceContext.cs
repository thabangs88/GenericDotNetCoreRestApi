using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GenericDotNetCoreRestApi.Model.Request;
using Microsoft.EntityFrameworkCore;

namespace GenericDotNetCoreRestApi.Model.Context
{
    public class MasterServiceContext : DbContext
    {
        public MasterServiceContext(DbContextOptions<MasterServiceContext> options)
: base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<App> Apps { get; set; }
        public DbSet<Company> Companies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var UsersConfig = modelBuilder.Entity<User>();
            UsersConfig.ToTable("Users");
            var AppsConfig = modelBuilder.Entity<App>();
            AppsConfig.ToTable("Apps");
            var CompaniesConfig = modelBuilder.Entity<Company>();
            CompaniesConfig.ToTable("Companies");

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Username).IsRequired();
            });

        }
    }
}
