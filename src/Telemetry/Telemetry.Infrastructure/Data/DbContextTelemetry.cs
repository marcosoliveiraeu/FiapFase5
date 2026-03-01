using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telemetry.Domain.Entities;

namespace Telemetry.Infrastructure.Data
{
    public class DbContextTelemetry : DbContext
    {

        public DbContextTelemetry(DbContextOptions<DbContextTelemetry> options) : base(options) { }

        public DbSet<LeituraSensor> LeiturasSensores => Set<LeituraSensor>();
        public DbSet<Alerta> Alertas => Set<Alerta>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("telemetria");

            // LEITURAS
            modelBuilder.Entity<LeituraSensor>(cfg =>
            {
                cfg.ToTable("LeiturasSensores");
                cfg.HasKey(x => x.Id);

                cfg.Property(x => x.Id)
                   .ValueGeneratedOnAdd(); // identity

                cfg.Property(x => x.TalhaoId)
                   .IsRequired();

                cfg.Property(x => x.DataHoraLeituraUtc)
                   .IsRequired();

                cfg.Property(x => x.UmidadeSolo)
                   .HasPrecision(5, 2) // 100.00
                   .IsRequired();

                cfg.Property(x => x.Temperatura)
                   .HasPrecision(5, 2)
                   .IsRequired();

                cfg.Property(x => x.Precipitacao)
               .HasPrecision(8, 2)
               .IsRequired();

                cfg.Property(x => x.RecebidoEmUtc)
                   .IsRequired();

                // Índice para séries temporais por talhão (Grafana e regra das 24h)
                cfg.HasIndex(x => new { x.TalhaoId, x.DataHoraLeituraUtc })
                   .HasDatabaseName("IX_Leituras_Talhao_DataHora");
            });

            // ALERTAS
            modelBuilder.Entity<Alerta>(cfg =>
            {
                cfg.ToTable("Alertas");
                cfg.HasKey(x => x.Id);

                cfg.Property(x => x.Id)
                   .ValueGeneratedOnAdd();

                cfg.Property(x => x.TalhaoId)
                   .IsRequired();

                cfg.Property(x => x.Tipo)
                   .HasConversion<int>()  // enum -> int
                   .IsRequired();

                cfg.Property(x => x.Status)
                   .HasConversion<int>()  // enum -> int
                   .IsRequired();

                cfg.Property(x => x.InicioUtc)
                   .IsRequired();

                cfg.Property(x => x.CriadoEmUtc)
                   .IsRequired();

                cfg.Property(x => x.Detalhes)
                   .HasMaxLength(2000);

                // Índices úteis para listar alertas abertos e por talhão
                cfg.HasIndex(x => new { x.TalhaoId, x.Status, x.CriadoEmUtc })
                   .HasDatabaseName("IX_Alertas_Talhao_Status_CriadoEm");

                cfg.HasIndex(x => new { x.Status, x.Tipo })
                   .HasDatabaseName("IX_Alertas_Status_Tipo");
            });

        }
    }
}
