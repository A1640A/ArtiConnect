using ArtiConnect.Api;
using ArtiConnect.Entities;
using ArtiConnect.Properties;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.DataAccess
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() : base("name=DefaultConnection")
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<AppDbContext>()); 
        }

        // Örnek entity'ler
        public DbSet<Ayar> Ayars { get; set; }
        public DbSet<ApiLog> ApiLogs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Gerekirse model konfigürasyonları burada yapılabilir
            base.OnModelCreating(modelBuilder);
        }
    }
}
