using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Telemetry.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialTelemetria : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "telemetria");

            migrationBuilder.CreateTable(
                name: "Alertas",
                schema: "telemetria",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TalhaoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Tipo = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    InicioUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FimUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CriadoEmUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechadoEmUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Detalhes = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alertas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LeiturasSensores",
                schema: "telemetria",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TalhaoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DataHoraLeituraUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UmidadeSolo = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    Temperatura = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    Precipitacao = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: false),
                    RecebidoEmUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeiturasSensores", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Alertas_Status_Tipo",
                schema: "telemetria",
                table: "Alertas",
                columns: new[] { "Status", "Tipo" });

            migrationBuilder.CreateIndex(
                name: "IX_Alertas_Talhao_Status_CriadoEm",
                schema: "telemetria",
                table: "Alertas",
                columns: new[] { "TalhaoId", "Status", "CriadoEmUtc" });

            migrationBuilder.CreateIndex(
                name: "IX_Leituras_Talhao_DataHora",
                schema: "telemetria",
                table: "LeiturasSensores",
                columns: new[] { "TalhaoId", "DataHoraLeituraUtc" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Alertas",
                schema: "telemetria");

            migrationBuilder.DropTable(
                name: "LeiturasSensores",
                schema: "telemetria");
        }
    }
}
