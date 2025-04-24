using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using GestionPaiement.Data;
using GestionPaiement.Models.DataModel;
using GestionPaiement.Models;
using GestionPaiement.Repository;
using GestionPaiement.Service;

namespace GestionPaiement.Controllers
{
    [Authorize]
    public class BulletinDeSalairesController : Controller
    {
        private readonly IBulletinDeSalaireRepository _repoBulletinDeSalaireRepository;
        private readonly IAgentRepository _repoAgentRepository;
        private readonly IEmailService _emailService;
        private readonly RoleController _roleManager;

        public BulletinDeSalairesController(
            IBulletinDeSalaireRepository repoBulletinDeSalaireRepository,
            IAgentRepository repoAgentRepository,
            IEmailService emailService,
            RoleController roleManager)
        {
            _repoBulletinDeSalaireRepository = repoBulletinDeSalaireRepository;
            _repoAgentRepository = repoAgentRepository;
            _emailService = emailService;
            _roleManager = roleManager;
        }

        // Action Index pour afficher les bulletins et gérer la recherche
        public async Task<IActionResult> Index(string searchQuery)
        {
            // Si une requête de recherche est fournie, on filtre les résultats
            if (!string.IsNullOrEmpty(searchQuery))
            {
                // Récupérer tous les bulletins de salaire
                var bulletins = await _repoBulletinDeSalaireRepository.GetAll();
                var result = bulletins.Where(b =>
                    b.Agent.Nom.Contains(searchQuery, StringComparison.OrdinalIgnoreCase) ||
                    b.Agent.Prenom.Contains(searchQuery, StringComparison.OrdinalIgnoreCase) ||
                    b.Date.ToString("MMMM yyyy").Contains(searchQuery, StringComparison.OrdinalIgnoreCase)
                ).ToList();

                // Ajouter la requête de recherche à ViewData pour afficher dans la barre de recherche
                ViewData["SearchQuery"] = searchQuery;
                return View(result);
            }
            else
            {
                // Si aucune recherche n'est effectuée, on affiche tous les bulletins
                ViewData["SearchQuery"] = null;
                return View(await _repoBulletinDeSalaireRepository.GetAll());
            }
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int id)
        {
            if (id == 0) return NotFound();

            var bulletin = await _repoBulletinDeSalaireRepository.GetById(id);
            if (bulletin == null) return NotFound();

            return View(bulletin);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateAsync()
        {
            var agents = await _repoAgentRepository.GetAll();
            ViewData["AgentId"] = new SelectList(agents, "IdAgent", "Nom");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdBulletin,AgentId,Date,SalaireBrut,SalaireNet")] BulletinDeSalaire bulletin)
        {
            if (ModelState.Count() > 0)
            {
                var agent = await _repoAgentRepository.GetById(bulletin.AgentId);
                if (agent != null && !string.IsNullOrWhiteSpace(agent.Email))
                {
                    bulletin.Agent = agent;
                    bulletin.GenererBulletin();

                    await _repoBulletinDeSalaireRepository.AddAsync(bulletin);

                    var email = new EmailIdentity
                    {
                        MailDeAdresse = "rh@tonentreprise.com",
                        MailAAdresse = agent.Email,
                        Sujet = $"Bulletin de Salaire - {bulletin.Date:MMMM yyyy}",
                        EmailcorpsMessage = $@"
                            Bonjour {agent.Prenom},

                            Veuillez trouver ci-dessous votre bulletin de salaire :

                            - Salaire Brut : {bulletin.SalaireBrut:N0} FCFA
                            - Salaire Net : {bulletin.SalaireNet:N0} FCFA

                            Cordialement,
                            Service Comptable
                        "
                    };

                    await _emailService.EnvoyerEmailAsync(email);
                }

                return RedirectToAction(nameof(Index));
            }

            var agents = await _repoAgentRepository.GetAll();
            ViewData["AgentId"] = new SelectList(agents, "IdAgent", "Nom");
            return View(bulletin);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var bulletin = await _repoBulletinDeSalaireRepository.GetById(id);
            if (bulletin == null) return NotFound();

            var agents = await _repoAgentRepository.GetAll();
            ViewData["AgentId"] = new SelectList(agents, "IdAgent", "Nom");
            return View(bulletin);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdBulletin,AgentId,Date,SalaireBrut,SalaireNet")] BulletinDeSalaire bulletin)
        {
            if (id != bulletin.IdBulletin) return NotFound();

            if (ModelState.Count() > 0)
            {
                try
                {
                    await _repoBulletinDeSalaireRepository.Update(id, bulletin);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (await _repoBulletinDeSalaireRepository.GetById(id) == null)
                        return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }

            var agents = await _repoAgentRepository.GetAll();
            ViewData["AgentId"] = new SelectList(agents, "IdAgent", "Nom");
            return View(bulletin);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var bulletin = await _repoBulletinDeSalaireRepository.GetById(id);
            if (bulletin == null) return NotFound();

            return View(bulletin);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bulletin = await _repoBulletinDeSalaireRepository.GetById(id);
            if (bulletin == null) return NotFound();

            await _repoBulletinDeSalaireRepository.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EnvoyerEmail(int id)
        {
            var bulletin = await _repoBulletinDeSalaireRepository.GetById(id);
            if (bulletin == null) return NotFound();

            var agent = await _repoAgentRepository.GetById(bulletin.AgentId);
            if (agent == null || string.IsNullOrWhiteSpace(agent.Email))
                return NotFound("Agent introuvable ou adresse email manquante.");

            var email = new EmailIdentity
            {
                MailDeAdresse = "rh@tonentreprise.com",
                MailAAdresse = agent.Email,
                Sujet = $"Bulletin de Salaire - {bulletin.Date:MMMM yyyy}",
                EmailcorpsMessage = $@"
                    Bonjour {agent.Prenom},

                    Veuillez trouver ci-dessous votre bulletin de salaire :

                    - Salaire Brut : {bulletin.SalaireBrut:N0} FCFA
                    - Salaire Net : {bulletin.SalaireNet:N0} FCFA

                    Cordialement,
                    Service Comptable
                "
            };

            await _emailService.EnvoyerEmailAsync(email);
            return RedirectToAction(nameof(Index));
        }
    }
}
