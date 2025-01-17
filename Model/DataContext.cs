﻿using Microsoft.EntityFrameworkCore;
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
        public DbSet<ClientePJ> ClientePJ { get; set; }
        public DbSet<ClientePFHonorario> ClientePFHonorarios { get; set; }
        public DbSet<ClientePJHonorario> ClientePJHonorarios { get; set; }
        public DbSet<PagamentoHonorario> PagamentosHonorarios { get; set; }

    }
}
