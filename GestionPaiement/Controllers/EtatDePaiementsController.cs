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
    public class EtatDePaiementsController : Controller
    {
        private readonly IEtatDePaiementRepository _repoEtatDePaiementRepository;
        private readonly IAgentRepository _repoAgentRepository;
        private readonly RoleController _roleManager;

        public EtatDePaiementsController(IEtatDePaiementRepository repoEtatDePaiementRepository,
                                        IAgentRepository repoAgentRepository)
        {
            _repoEtatDePaiementRepository = repoEtatDePaiementRepository;
            _repoAgentRepository = repoAgentRepository;
        }

        // GET: EtatDePaiements
        public async Task<IActionResult> Index()
        {
            return View(await _repoEtatDePaiementRepository.GetAll());
        }

        [Authorize(Roles = "Admin")]
        // GET: EtatDePaiements/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var etatDePaiement = await _repoEtatDePaiementRepository.GetById(id);
            if (etatDePaiement == null)
            {
                return NotFound();
            }

            return View(etatDePaiement);
        }

        [Authorize(Roles = "Admin")]
        // GET: EtatDePaiements/Create
        public async Task<IActionResult> CreateAsync()
        {
            var lstAgent = await _repoAgentRepository.GetAll();

            ViewData["AgentId"] = new SelectList(lstAgent, "IdAgent", "Nom");
            return View();
        }

        // POST: EtatDePaiements/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdEtat,AgentId,DateDebut,DateFin,TotalPaye")] EtatDePaiement etatDePaiement)
        {
            if (ModelState.Count() > 0)
            {
                await _repoEtatDePaiementRepository.AddAsync(etatDePaiement);
                return RedirectToAction(nameof(Index));
            }
            var lstAgent = await _repoAgentRepository.GetAll();

            ViewData["AgentId"] = new SelectList(lstAgent, "IdAgent", "Nom");
            return View(etatDePaiement);
        }

        [Authorize(Roles = "Admin")]
        // GET: EtatDePaiements/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var etatDePaiement = await _repoEtatDePaiementRepository.GetById(id);
            if (etatDePaiement == null)
            {
                return NotFound();
            }
            var lstAgent = await _repoAgentRepository.GetAll();

            ViewData["AgentId"] = new SelectList(lstAgent, "IdAgent", "Nom");
            return View(etatDePaiement);
        }

        // POST: EtatDePaiements/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdEtat,AgentId,DateDebut,DateFin,TotalPaye")] EtatDePaiement etatDePaiement)
        {
            if (id != etatDePaiement.IdEtat)
            {
                return NotFound();
            }

            if (ModelState.Count() > 0)
            {
                try
                {
                    await _repoEtatDePaiementRepository.Update(id, etatDePaiement);
                }
                catch (DbUpdateConcurrencyException)
                {
                    var existingBulletinDeSalaire = await _repoEtatDePaiementRepository.GetById(id);
                    if (existingBulletinDeSalaire == null)
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
            return View(etatDePaiement);
        }

        [Authorize(Roles = "Admin")]
        // GET: EtatDePaiements/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var etatDePaiement = await _repoEtatDePaiementRepository.GetById(id);
            if (etatDePaiement == null)
            {
                return NotFound();
            }

            return View(etatDePaiement);
        }

        // POST: EtatDePaiements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var etatDePaiement = await _repoEtatDePaiementRepository.GetById(id);
            if (etatDePaiement == null)
            {
                return NotFound();
            }

            await _repoEtatDePaiementRepository.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
 }
