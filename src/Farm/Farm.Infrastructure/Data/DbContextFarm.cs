using Farm.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Farm.Infrastructure.Data
{
    public class DbContextFarm : DbContext
    {
        public DbContextFarm(DbContextOptions<DbContextFarm> options) : base(options) { }

        public DbSet<Propriedade> Propriedades => Set<Propriedade>();
        public DbSet<Talhao> Talhoes => Set<Talhao>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("farm");

            modelBuilder.Entity<Propriedade>(cfg =>
            {
                cfg.ToTable("Propriedades");
                cfg.HasKey(x => x.Id);

                cfg.Property(x => x.Nome).HasMaxLength(120).IsRequired();
                cfg.Property(x => x.Cidade).HasMaxLength(120);
                cfg.Property(x => x.Estado).HasMaxLength(2);

                cfg.HasIndex(x => x.UsuarioId); // filtrar rápido “minhas propriedades”

                cfg.HasMany(x => x.Talhoes)
                   .WithOne(x => x.Propriedade)
                   .HasForeignKey(x => x.PropriedadeId)
                   .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Talhao>(cfg =>
            {
                cfg.ToTable("Talhoes");
                cfg.HasKey(x => x.Id);

                cfg.Property(x => x.Nome).HasMaxLength(120).IsRequired();
                cfg.Property(x => x.Cultura).HasMaxLength(80);

                cfg.HasIndex(x => x.PropriedadeId);

                // evitar talhão com mesmo nome dentro da mesma propriedade
                cfg.HasIndex(x => new { x.PropriedadeId, x.Nome }).IsUnique();
            });
        }

    }
}
