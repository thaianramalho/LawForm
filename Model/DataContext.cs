using Microsoft.EntityFrameworkCore;
using SQLitePCL;

namespace LawForm.Model
{
    public class DataContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            Batteries.Init();
            optionsBuilder.UseSqlite("Data source=Database.db");
        }

        public DbSet<User> User { get; set; }
        public DbSet<Advogado> Advogado { get; set; }
        public DbSet<ClientePF> ClientePF { get; set; }
        public object? Users { get; set; }
        public object? Advogados { get; set; }
        public object? ClientesPF { get; set; }
    }
}
