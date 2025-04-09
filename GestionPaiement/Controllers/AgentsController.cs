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
using Microsoft.AspNetCore.Identity;  // Ajout de cette ligne pour utiliser UserManager

namespace GestionPaiement.Controllers
{
    [Authorize]
    public class AgentsController : Controller
    {
        private readonly IAgentRepository _repoAgentRepository;
        private readonly RoleController _roleManager;
       

        public AgentsController(IAgentRepository repoAgentRepository)
        {
            _repoAgentRepository = repoAgentRepository;
            
        }

        // GET: Agents
        public async Task<IActionResult> Index()
        {
            var listAgent = await _repoAgentRepository.GetAll();
            return View(listAgent);
        }


        [Authorize(Roles = "Admin")]
        // GET: Agents/Details/5 (accessible uniquement par un administrateur)
        public async Task<IActionResult> Details(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var agent = await _repoAgentRepository.GetById(id);
            if (agent == null)
            {
                return NotFound();
            }

            return View(agent);
        }

        [Authorize(Roles = "Admin")]
        // GET: Agents/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Agents/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdAgent,Nom,Prenom,Email,DateEmbauche,Poste,SalaireBrut")] Agent agent)
        {
            if (ModelState.Count() > 0)
            {
                await _repoAgentRepository.AddAsync(agent);
                return RedirectToAction(nameof(Index));
            }
            return View(agent);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var agent = await _repoAgentRepository.GetById(id);
            if (agent == null)
            {
                return NotFound();
            }
            return View(agent);
        }

        // POST: Agents/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdAgent,Nom,Prenom,Email,DateEmbauche,Poste,SalaireBrut")] Agent agent)
        {
            if (id != agent.IdAgent)
            {
                return NotFound();
            }

            if (ModelState.Count() > 0)
            {
                try
                {
                    await _repoAgentRepository.Update(id, agent);
                }
                catch (DbUpdateConcurrencyException)
                {
                    var existingAgent = await _repoAgentRepository.GetById(id);
                    if (existingAgent == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw; // Relancer l'exception si une erreur de concurrence se produit
                    }
                }

                return RedirectToAction(nameof(Index)); // Rediriger vers la vue d'index après la mise à jour
            }
            return View(agent);
        }

        [Authorize(Roles = "Admin")]
        // GET: Agents/Delete/5 (accessible uniquement par un administrateur)
        public async Task<IActionResult> Delete(int id)
        {
            var agent = await _repoAgentRepository.GetById(id);
            if (agent == null)
            {
                return NotFound();
            }

            return View(agent);
        }

        // POST: Agents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var agent = await _repoAgentRepository.GetById(id);
            if (agent == null) { return NotFound(); }

            await _repoAgentRepository.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
