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
    public class BulletinDeSalairesController : Controller
    {
        private readonly IBulletinDeSalaireRepository _repoBulletinDeSalaireRepository;
        private readonly IAgentRepository _repoAgentRepository;
        private readonly RoleController _roleManager;

        public BulletinDeSalairesController(IBulletinDeSalaireRepository repoBulletinDeSalaireRepository, IAgentRepository repoAgentRepository, RoleController roleManager)
        {
            _repoBulletinDeSalaireRepository = repoBulletinDeSalaireRepository;
            _repoAgentRepository = repoAgentRepository;
            _roleManager = roleManager;
        }



        // GET: BulletinDeSalaires
        public async Task<IActionResult> Index()
        {
            return View(await _repoBulletinDeSalaireRepository.GetAll());
        }

        [Authorize(Roles = "Admin")]
        // GET: BulletinDeSalaires/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bulletinDeSalaire = await _repoBulletinDeSalaireRepository.GetById(id);
            if (bulletinDeSalaire == null)
            {
                return NotFound();
            }

            return View(bulletinDeSalaire);
        }

        [Authorize(Roles = "Admin")]
        // GET: BulletinDeSalaires/Create
        public async Task<IActionResult> CreateAsync()
        {
            var lstAgent = await _repoAgentRepository.GetAll();
       
            ViewData["AgentId"] = new SelectList(lstAgent, "IdAgent", "Nom");
            return View();
        }

        // POST: BulletinDeSalaires/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdBulletin,AgentId,Date,SalaireBrut,SalaireNet")] BulletinDeSalaire bulletinDeSalaire)
        {
            if (ModelState.Count() > 0)
            {
                await _repoBulletinDeSalaireRepository.AddAsync(bulletinDeSalaire);
                return RedirectToAction(nameof(Index));
            }
            var lstAgent = await _repoAgentRepository.GetAll();

            ViewData["AgentId"] = new SelectList(lstAgent, "IdAgent", "Nom");
            return View(bulletinDeSalaire);
        }

        [Authorize(Roles = "Admin")]
        // GET: BulletinDeSalaires/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bulletinDeSalaire = await _repoBulletinDeSalaireRepository.GetById(id);
            if (bulletinDeSalaire == null)
            {
                return NotFound();
            }
            var lstAgent = await _repoAgentRepository.GetAll();

            ViewData["AgentId"] = new SelectList(lstAgent, "IdAgent", "Nom");
            return View(bulletinDeSalaire);
        }

        // POST: BulletinDeSalaires/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdBulletin,AgentId,Date,SalaireBrut,SalaireNet")] BulletinDeSalaire bulletinDeSalaire)
        {
            if (id != bulletinDeSalaire.IdBulletin)
            {
                return NotFound();
            }

            if (ModelState.Count() > 0)
            {
                try
                {
                    await _repoBulletinDeSalaireRepository.Update(id, bulletinDeSalaire);
                }
                catch (DbUpdateConcurrencyException)
                {
                    var existingBulletinDeSalaire = await _repoBulletinDeSalaireRepository.GetById(id);
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
            return View(bulletinDeSalaire);
        }

        [Authorize(Roles = "Admin")]
        // GET: BulletinDeSalaires/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var bulletinDeSalaire = await _repoBulletinDeSalaireRepository.GetById(id);
            if (bulletinDeSalaire == null)
            {
                return NotFound();
            }

            return View(bulletinDeSalaire);
        }

        // POST: BulletinDeSalaires/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bulletinDeSalaire = await _repoBulletinDeSalaireRepository.GetById(id);
            if (bulletinDeSalaire == null)
            {
                return NotFound();
            }

            await _repoBulletinDeSalaireRepository.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
