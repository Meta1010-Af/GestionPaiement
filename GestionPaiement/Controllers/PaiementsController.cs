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
        private readonly RoleController _roleManager;

        public PaiementsController(IPaiementRepository repoPaiementRepository,
                                    IAgentRepository repoAgentRepository)
        {
            _repoPaiementRepository = repoPaiementRepository;
            _repoAgentRepository = repoAgentRepository;
        }

        // GET: Paiements
        public async Task<IActionResult> Index()
        {
            return View(await _repoPaiementRepository.GetAll());
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
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdPaiement,AgentId,MontantPaye,DatePaiement,ModeDePaiement")] Paiement paiement)
        {
            if (ModelState.Count() > 0)
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
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdPaiement,AgentId,MontantPaye,DatePaiement,ModeDePaiement")] Paiement paiement)
        {
            if (id != paiement.IdPaiement)
            {
                return NotFound();
            }

            if (ModelState.Count() > 0)
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
