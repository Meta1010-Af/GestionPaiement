using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using GestionPaiement.Data;
using GestionPaiement.Models.DataModel;
using GestionPaiement.Repository;

namespace GestionPaiement.Controllers
{
    [Authorize]
    public class AgentsController : Controller
    {
        private readonly IAgentRepository _repoAgentRepository;

        public AgentsController(IAgentRepository repoAgentRepository)
        {
            _repoAgentRepository = repoAgentRepository;
        }

        // GET: Agents
        public async Task<IActionResult> Index(string searchString)
        {
            ViewData["CurrentFilter"] = searchString;

            IEnumerable<Agent> agents;

            if (string.IsNullOrWhiteSpace(searchString))
            {
                agents = await _repoAgentRepository.GetAll();
            }
            else
            {
                agents = await _repoAgentRepository.SearchAgentsAsync(searchString);
            }

            return View(agents);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int id)
        {
            var agent = await _repoAgentRepository.GetById(id);
            if (agent == null)
            {
                return NotFound();
            }

            return View(agent);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdAgent,Nom,Prenom,Email,DateEmbauche,Poste,SalaireBrut")] Agent agent)
        {
            if (!User.IsInRole("Admin"))
            {
                agent.SalaireBrut = 0;
            }

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdAgent,Nom,Prenom,Email,DateEmbauche,Poste,SalaireBrut")] Agent agent)
        {
            if (id != agent.IdAgent)
            {
                return NotFound();
            }

            if (!User.IsInRole("Admin"))
            {
                var existingAgent = await _repoAgentRepository.GetById(id);
                if (existingAgent == null)
                {
                    return NotFound();
                }

                agent.SalaireBrut = existingAgent.SalaireBrut;
            }

            if (ModelState.Count() > 0)
            {
                try
                {
                    await _repoAgentRepository.Update(id, agent);
                }
                catch (DbUpdateConcurrencyException)
                {
                    var exists = await _repoAgentRepository.GetById(id);
                    if (exists == null)
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

            return View(agent);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var agent = await _repoAgentRepository.GetById(id);
            if (agent == null)
            {
                return NotFound();
            }

            return View(agent);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var agent = await _repoAgentRepository.GetById(id);
            if (agent == null)
            {
                return NotFound();
            }

            await _repoAgentRepository.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
