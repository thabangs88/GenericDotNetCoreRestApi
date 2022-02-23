using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GenericDotNetCoreRestApi.Model.Request;
using Microsoft.EntityFrameworkCore;

namespace GenericDotNetCoreRestApi.Model.Context
{
    public class MasterContext: DbContext
    {
        public MasterContext(DbContextOptions<MasterContext> options)
        : base(options)
        {
        }


        public DbSet<Client> Client { get; set; }
        public DbSet<ClientAddress> ClientAddress { get; set; }
        public DbSet<ClientContacts> ClientContacts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var ClientConfig = modelBuilder.Entity<Client>();
            ClientConfig.ToTable("Clients");

            var ClientAddressesConfig = modelBuilder.Entity<ClientAddress>();
            ClientAddressesConfig.ToTable("ClientAddress");

            var ClientContactsConfig = modelBuilder.Entity<ClientContacts>();
            ClientContactsConfig.ToTable("ClientContacts");

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Username).IsRequired();
            });

        }
    }
}
