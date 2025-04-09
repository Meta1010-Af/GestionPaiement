using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionPaiement.Data.Migrations
{
    /// <inheritdoc />
    public partial class Azou : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Agents",
                columns: table => new
                {
                    IdAgent = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nom = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Prenom = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateEmbauche = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Poste = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SalaireBrut = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Agents", x => x.IdAgent);
                });

            migrationBuilder.CreateTable(
                name: "BulletinDeSalaires",
                columns: table => new
                {
                    IdBulletin = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AgentId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SalaireBrut = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SalaireNet = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BulletinDeSalaires", x => x.IdBulletin);
                    table.ForeignKey(
                        name: "FK_BulletinDeSalaires_Agents_AgentId",
                        column: x => x.AgentId,
                        principalTable: "Agents",
                        principalColumn: "IdAgent",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EtatDePaiements",
                columns: table => new
                {
                    IdEtat = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AgentId = table.Column<int>(type: "int", nullable: false),
                    DateDebut = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateFin = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalPaye = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EtatDePaiements", x => x.IdEtat);
                    table.ForeignKey(
                        name: "FK_EtatDePaiements_Agents_AgentId",
                        column: x => x.AgentId,
                        principalTable: "Agents",
                        principalColumn: "IdAgent",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Rubriques",
                columns: table => new
                {
                    IdRubrique = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nom = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Montant = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AgentIdAgent = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rubriques", x => x.IdRubrique);
                    table.ForeignKey(
                        name: "FK_Rubriques_Agents_AgentIdAgent",
                        column: x => x.AgentIdAgent,
                        principalTable: "Agents",
                        principalColumn: "IdAgent");
                });

            migrationBuilder.CreateTable(
                name: "Salaires",
                columns: table => new
                {
                    IdSalaire = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AgentId = table.Column<int>(type: "int", nullable: false),
                    SalaireBrut = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SalaireNet = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DateDePaie = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Salaires", x => x.IdSalaire);
                    table.ForeignKey(
                        name: "FK_Salaires_Agents_AgentId",
                        column: x => x.AgentId,
                        principalTable: "Agents",
                        principalColumn: "IdAgent",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Paiements",
                columns: table => new
                {
                    IdPaiement = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AgentId = table.Column<int>(type: "int", nullable: false),
                    MontantPaye = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DatePaiement = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModeDePaiement = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EtatDePaiementIdEtat = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Paiements", x => x.IdPaiement);
                    table.ForeignKey(
                        name: "FK_Paiements_Agents_AgentId",
                        column: x => x.AgentId,
                        principalTable: "Agents",
                        principalColumn: "IdAgent",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Paiements_EtatDePaiements_EtatDePaiementIdEtat",
                        column: x => x.EtatDePaiementIdEtat,
                        principalTable: "EtatDePaiements",
                        principalColumn: "IdEtat");
                });

            migrationBuilder.CreateIndex(
                name: "IX_BulletinDeSalaires_AgentId",
                table: "BulletinDeSalaires",
                column: "AgentId");

            migrationBuilder.CreateIndex(
                name: "IX_EtatDePaiements_AgentId",
                table: "EtatDePaiements",
                column: "AgentId");

            migrationBuilder.CreateIndex(
                name: "IX_Paiements_AgentId",
                table: "Paiements",
                column: "AgentId");

            migrationBuilder.CreateIndex(
                name: "IX_Paiements_EtatDePaiementIdEtat",
                table: "Paiements",
                column: "EtatDePaiementIdEtat");

            migrationBuilder.CreateIndex(
                name: "IX_Rubriques_AgentIdAgent",
                table: "Rubriques",
                column: "AgentIdAgent");

            migrationBuilder.CreateIndex(
                name: "IX_Salaires_AgentId",
                table: "Salaires",
                column: "AgentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BulletinDeSalaires");

            migrationBuilder.DropTable(
                name: "Paiements");

            migrationBuilder.DropTable(
                name: "Rubriques");

            migrationBuilder.DropTable(
                name: "Salaires");

            migrationBuilder.DropTable(
                name: "EtatDePaiements");

            migrationBuilder.DropTable(
                name: "Agents");
        }
    }
}
