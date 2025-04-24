using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GestionPaiement.Data;
using GestionPaiement.Models.DataModel;
using Microsoft.AspNetCore.Authorization;
using GestionPaiement.Repository;

namespace GestionPaiement.Controllers
{
    [Authorize]
    public class PaiementsController : Controller
    {
        private readonly IPaiementRepository _repoPaiementRepository;
        private readonly IAgentRepository _repoAgentRepository;

        public PaiementsController(IPaiementRepository repoPaiementRepository,
                                    IAgentRepository repoAgentRepository)
        {
            _repoPaiementRepository = repoPaiementRepository;
            _repoAgentRepository = repoAgentRepository;
        }

        // GET: Paiements
        public async Task<IActionResult> Index(string search)
        {
            // Récupère tous les paiements
            var paiements = await _repoPaiementRepository.GetAll();

            // Si un terme de recherche est fourni, filtre les paiements
            if (!string.IsNullOrEmpty(search))
            {
                paiements = paiements.Where(p =>
                    p.Agent.Prenom.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                    p.Agent.Nom.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                    p.ModeDePaiement.Contains(search, StringComparison.OrdinalIgnoreCase)
                ).ToList();
            }

            return View(paiements);
        }

        [Authorize(Roles = "Admin")]
        // GET: Paiements/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var paiement = await _repoPaiementRepository.GetById(id);
            if (paiement == null)
            {
                return NotFound();
            }

            return View(paiement);
        }

        [Authorize(Roles = "Admin")]
        // GET: Paiements/Create
        public async Task<IActionResult> CreateAsync()
        {
            var lstAgent = await _repoAgentRepository.GetAll();
            ViewData["AgentId"] = new SelectList(lstAgent, "IdAgent", "Nom");
            return View();
        }

        // POST: Paiements/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdPaiement,AgentId,MontantPaye,DatePaiement,ModeDePaiement")] Paiement paiement)
        {
            if (ModelState.IsValid)
            {
                await _repoPaiementRepository.AddAsync(paiement);
                return RedirectToAction(nameof(Index));
            }
            var lstAgent = await _repoAgentRepository.GetAll();
            ViewData["AgentId"] = new SelectList(lstAgent, "IdAgent", "Nom");
            return View(paiement);
        }

        [Authorize(Roles = "Admin")]
        // GET: Paiements/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var paiement = await _repoPaiementRepository.GetById(id);
            if (paiement == null)
            {
                return NotFound();
            }

            var lstAgent = await _repoAgentRepository.GetAll();
            ViewData["AgentId"] = new SelectList(lstAgent, "IdAgent", "Nom");
            return View(paiement);
        }

        // POST: Paiements/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdPaiement,AgentId,MontantPaye,DatePaiement,ModeDePaiement")] Paiement paiement)
        {
            if (id != paiement.IdPaiement)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _repoPaiementRepository.Update(id, paiement);
                }
                catch (DbUpdateConcurrencyException)
                {
                    var existingPaiement = await _repoPaiementRepository.GetById(id);
                    if (existingPaiement == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            var lstAgent = await _repoAgentRepository.GetAll();
            ViewData["AgentId"] = new SelectList(lstAgent, "IdAgent", "Nom");
            return View(paiement);
        }

        [Authorize(Roles = "Admin")]
        // GET: Paiements/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var paiement = await _repoPaiementRepository.GetById(id);
            if (paiement == null)
            {
                return NotFound();
            }

            return View(paiement);
        }

        // POST: Paiements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var paiement = await _repoPaiementRepository.GetById(id);
            if (paiement == null)
            {
                return NotFound();
            }

            await _repoPaiementRepository.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
