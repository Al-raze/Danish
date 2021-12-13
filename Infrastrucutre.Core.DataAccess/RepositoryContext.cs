using Infrastructure.ConfigurationProvider;
using Infrastrucutre.Core.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace Infrastrucutre.Core.DataAccess
{
    public class RepositoryContext : DbContext
    {
        public RepositoryContext()
            : base("DefaultConnection")
        {

        }
        public DbSet<ItemMaster> Items { get; set; }
        
        public DbSet<Supplier> Suppliers { get; set; }

        public DbSet<ItemCategory> ItemCategories { get; set; }

        public DbSet<ItemManufacturer> ItemManufacturers { get; set; }

        public DbSet<ItemColor> ItemColors { get; set; }

    }
}
