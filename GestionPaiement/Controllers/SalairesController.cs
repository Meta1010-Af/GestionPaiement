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
    public class SalairesController : Controller
    {
        private readonly ISalaireRepository _repoSalaireRepository;
        private readonly IAgentRepository _repoAgentRepository;
        private readonly RoleController _roleManager;

        public SalairesController(ISalaireRepository repoSalaireRepository, 
                                    IAgentRepository repoAgentRepository)
        {
            _repoSalaireRepository = repoSalaireRepository;
            _repoAgentRepository = repoAgentRepository;
        }

        // GET: Salaires
        public async Task<IActionResult> Index()
        {
            return View(await _repoSalaireRepository.GetAll());
        }

        [Authorize(Roles = "Admin")]
        // GET: Salaires/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salaire = await _repoSalaireRepository.GetById(id);
            if (salaire == null)
            {
                return NotFound();
            }

            return View(salaire);
        }

        [Authorize(Roles = "Admin")]
        // GET: Salaires/Create
        public async Task<IActionResult> CreateAsync()
        {
            var lstAgent = await _repoAgentRepository.GetAll();

            ViewData["AgentId"] = new SelectList(lstAgent, "IdAgent", "Nom");
            return View();
        }

        // POST: Salaires/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdSalaire,AgentId,SalaireBrut,SalaireNet,DateDePaie")] Salaire salaire)
        {
            if (ModelState.Count() > 0)
            {
                await _repoSalaireRepository.AddAsync(salaire);
                return RedirectToAction(nameof(Index));
            }
            var lstAgent = await _repoAgentRepository.GetAll();

            ViewData["AgentId"] = new SelectList(lstAgent, "IdAgent", "Nom");
            return View(salaire);
        }

        [Authorize(Roles = "Admin")]
        // GET: Salaires/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salaire = await _repoSalaireRepository.GetById(id);
            if (salaire == null)
            {
                return NotFound();
            }
            var lstAgent = await _repoAgentRepository.GetAll();

            ViewData["AgentId"] = new SelectList(lstAgent, "IdAgent", "Nom");
            return View(salaire);
        }

        // POST: Salaires/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdSalaire,AgentId,SalaireBrut,SalaireNet,DateDePaie")] Salaire salaire)
        {
            if (id != salaire.IdSalaire)
            {
                return NotFound();
            }

            if (ModelState.Count() > 0)
            {
                try
                {
                    await _repoSalaireRepository.Update(id, salaire);
                }
                catch (DbUpdateConcurrencyException)
                {
                    var existingSalaire = await _repoSalaireRepository.GetById(id);
                    if (existingSalaire == null)
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
            return View(salaire);
        }

        [Authorize(Roles = "Admin")]
        // GET: Salaires/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var salaire = await _repoSalaireRepository.GetById(id);
            if (salaire == null)
            {
                return NotFound();
            }

            return View(salaire);
        }

        // POST: Salaires/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var salaire = await _repoSalaireRepository.GetById(id);
            if (salaire == null)
            {
                return NotFound();
            }

            await _repoSalaireRepository.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
