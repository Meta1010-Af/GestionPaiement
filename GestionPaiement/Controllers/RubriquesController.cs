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
    public class RubriquesController : Controller
    {
        private readonly IRubriqueRepository _repoRubriqueRepository;

        public RubriquesController(IRubriqueRepository repoRubriqueRepository)
        {
            _repoRubriqueRepository = repoRubriqueRepository;
        }

        // GET: Rubriques
        public async Task<IActionResult> Index()
        {
            var listRubrique = await _repoRubriqueRepository.GetAll();
            return View(listRubrique);
        }

        // GET: Rubriques/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rubrique = await _repoRubriqueRepository.GetById(id);
            if (rubrique == null)
            {
                return NotFound();
            }

            return View(rubrique);
        }

        // GET: Rubriques/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Rubriques/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdRubrique,Nom,Montant,Type")] Rubrique rubrique)
        {
            if (ModelState.Count() > 0)
            {
                await _repoRubriqueRepository.AddAsync(rubrique);
                return RedirectToAction(nameof(Index));
            }
            return View(rubrique);
        }

        // GET: Rubriques/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var rubrique = await _repoRubriqueRepository.GetById(id);
            if (rubrique == null)
            {
                return NotFound();
            }
            return View(rubrique);
        }

        // POST: Rubriques/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdRubrique,Nom,Montant,Type")] Rubrique rubrique)
        {
            if (id != rubrique.IdRubrique)
            {
                return NotFound();
            }

            if (ModelState.Count() > 0)
            {
                try
                {
                    // Mettre à jour la formation en utilisant le repository
                    await _repoRubriqueRepository.Update(id, rubrique);
                }
                catch (DbUpdateConcurrencyException)
                {
                    var existingRubrique = await _repoRubriqueRepository.GetById(id);
                    if (existingRubrique == null)
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
            return View(rubrique); // Renvoyer la vue avec le modèle si l'état est invalide
        }

        // GET: Rubriques/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var rubrique = await _repoRubriqueRepository.GetById(id);
            if (rubrique == null)
            {
                return NotFound();
            }

            return View(rubrique);
        }

        // POST: Rubriques/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var rubrique = await _repoRubriqueRepository.GetById(id);
            if (rubrique == null) { return NotFound(); }

            await _repoRubriqueRepository.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
