using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Farm.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialFarm : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "farm");

            migrationBuilder.CreateTable(
                name: "Propriedades",
                schema: "farm",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    Cidade = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: true),
                    Estado = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    CriadoEmUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Propriedades", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Talhoes",
                schema: "farm",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PropriedadeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    Cultura = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    AreaHectares = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CriadoEmUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Talhoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Talhoes_Propriedades_PropriedadeId",
                        column: x => x.PropriedadeId,
                        principalSchema: "farm",
                        principalTable: "Propriedades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Propriedades_UsuarioId",
                schema: "farm",
                table: "Propriedades",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Talhoes_PropriedadeId",
                schema: "farm",
                table: "Talhoes",
                column: "PropriedadeId");

            migrationBuilder.CreateIndex(
                name: "IX_Talhoes_PropriedadeId_Nome",
                schema: "farm",
                table: "Talhoes",
                columns: new[] { "PropriedadeId", "Nome" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Talhoes",
                schema: "farm");

            migrationBuilder.DropTable(
                name: "Propriedades",
                schema: "farm");
        }
    }
}
