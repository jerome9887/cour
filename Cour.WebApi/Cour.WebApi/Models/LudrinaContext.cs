using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Cour.WebApi.Models
{
    public class CourContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public CourContext() : base("name=CourContext")
        {
        }

        public System.Data.Entity.DbSet<User> Users { get; set; }
        public System.Data.Entity.DbSet<Company> Companies { get; set; }
        public System.Data.Entity.DbSet<Port> Ports { get; set; }
        public System.Data.Entity.DbSet<DeliverySequence> DeliverySequences { get; set; }
        public System.Data.Entity.DbSet<Order> Orders { get; set; }
        public System.Data.Entity.DbSet<OrderDetail> OrderDetails { get; set; }
        public System.Data.Entity.DbSet<Item> Items { get; set; }

    }
}
