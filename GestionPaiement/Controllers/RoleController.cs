using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GestionPaiement.Controllers
{
    [Authorize]
    public class RoleController : Controller
    {
        private RoleManager<IdentityRole> _roleManager;

        public RoleController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            var roles = _roleManager.Roles.ToList();
            return View(roles);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult AjouterRole()
        {
            return View(new IdentityRole());
        }
        [HttpPost]
        public async Task<IActionResult> AjouterRole(IdentityRole role)
        {
            await _roleManager.CreateAsync(role);
            return View();
        }
    }
}
