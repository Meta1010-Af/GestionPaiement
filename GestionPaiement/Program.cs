using GestionPaiement.Controllers;
using GestionPaiement.Data;
using GestionPaiement.Models;
using GestionPaiement.Repository;
using GestionPaiement.Service; // Assurez-vous d'inclure cet espace de noms pour IEmailService et EmailService
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Configuration de la cha�ne de connexion pour la base de donn�es
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Configuration de l'authentification et des r�les
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

// Enregistrement des repositories
builder.Services.AddScoped<IAgentRepository, AgentRepository>();
builder.Services.AddScoped<IBulletinDeSalaireRepository, BulletinDeSalaireRepository>();
builder.Services.AddScoped<IEtatDePaiementRepository, EtatDePaiementRepository>();
builder.Services.AddScoped<IPaiementRepository, PaiementRepository>();
builder.Services.AddScoped<IRubriqueRepository, RubriqueRepository>();
builder.Services.AddScoped<ISalaireRepository, SalaireRepository>();
builder.Services.AddScoped<RoleController, RoleController>();

// Configuration du service d'email
builder.Services.AddScoped<IEmailService, EmailService>();

// Configuration pour r�cup�rer les param�tres SMTP
builder.Services.Configure<SMTPSettings>(builder.Configuration.GetSection("SMTPSettings"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
