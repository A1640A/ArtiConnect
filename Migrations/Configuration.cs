using ArtiConnect.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Data.SQLite.EF6.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtiConnect.Migrations
{
    public sealed class Configuration : DbMigrationsConfiguration<ArtiConnect.DataAccess.AppDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
             
            SetSqlGenerator("System.Data.SQLite", new SQLiteMigrationSqlGenerator());
        }

        protected override void Seed(ArtiConnect.DataAccess.AppDbContext context)
        {
            context.Ayars.AddOrUpdate(
                s => s.Id,
                new Ayar { Port = "9000" }
            );
        }
    }
}
