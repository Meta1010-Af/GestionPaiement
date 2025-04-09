using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionPaiement.Data.Migrations
{
    /// <inheritdoc />
    public partial class zee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TableauDeBord",
                columns: table => new
                {
                    TotalAgents = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TotalSalaires = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalPaiements = table.Column<int>(type: "int", nullable: false),
                    TotalBulletinsDeSalaire = table.Column<int>(type: "int", nullable: false),
                    TotalRubriques = table.Column<int>(type: "int", nullable: false),
                    TotalEtatDePaiement = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TableauDeBord", x => x.TotalAgents);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TableauDeBord");
        }
    }
}
