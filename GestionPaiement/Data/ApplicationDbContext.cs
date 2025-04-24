using GestionPaiement.Models.DataModel;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GestionPaiement.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    public DbSet<Agent> Agents { get; set; }
    public DbSet<BulletinDeSalaire> BulletinDeSalaires { get; set; }
    public DbSet<EtatDePaiement> EtatDePaiements { get; set; }
    public DbSet<Paiement> Paiements { get; set; }
    public DbSet<Rubrique> Rubriques { get; set; }
    public DbSet<Salaire> Salaires { get; set; }
    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<EtatDePaiement>()
            .HasOne(e => e.Agent)                // Un EtatDePaiement a un Agent
            .WithMany(a => a.EtatDePaiements)    // Un Agent peut avoir plusieurs EtatsDePaiement
            .HasForeignKey(e => e.AgentId)       // Définir explicitement la clé étrangère
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<BulletinDeSalaire>()
            .HasOne(b => b.Agent)               // Une bulletin de salaire a un agent
            .WithMany(a => a.BulletinDeSalaires) // Un agent peut avoir plusieurs bulletins de salaire
            .HasForeignKey(b => b.AgentId)       // Utiliser AgentId comme clé étrangère
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Paiement>()
            .HasOne(m => m.Agent)               // Un Paiement a un Agent
            .WithMany(am => am.Paiements)       // Un Agent peut avoir plusieurs paiements
            .HasForeignKey(m => m.AgentId)      // Utiliser AgentId comme clé étrangère
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Salaire>()
            .HasOne(m => m.Agent)               // Un Salaire a un Agent
            .WithMany(am => am.Salaires)        // Un Agent peut avoir plusieurs salaires
            .HasForeignKey(m => m.AgentId)      // Utiliser AgentId comme clé étrangère
            .OnDelete(DeleteBehavior.Cascade);

        base.OnModelCreating(modelBuilder);
    }
}
