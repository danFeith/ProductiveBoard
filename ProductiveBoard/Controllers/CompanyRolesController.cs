using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProductiveBoard.Data;
using ProductiveBoard.Models;

namespace ProductiveBoard.Controllers
{
    public class CompanyRolesController : Controller
    {
        private readonly ILogger<CompanyRolesController> _logger;
        private readonly ApplicationDbContext _context;

        public CompanyRolesController(ILogger<CompanyRolesController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        // GET /CompanyRoles
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _context.CompanyRoles.ToListAsync());
        }

        // GET /CompanyRoles/Details/{id}
        [HttpGet]
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            CompanyRole role = await _context.CompanyRoles.FirstOrDefaultAsync(e => e.Id == id);
            if (role == null)
            {
                return NotFound();
            }

            return View(role);
        }

        // POST /CompanyRoles
        [HttpPost]
        public async Task<IActionResult> Create(CompanyRole role)
        {
            _context.Add(role);
            await _context.SaveChangesAsync();
            return View(role);
        }

        // PUT /CompanyRoles/Update
        [HttpPut]
        public async Task<IActionResult> Update(CompanyRole role)
        {
            _context.Update(role);
            await _context.SaveChangesAsync();
            return View(role);
        }

        // DELETE /CompanyRoles/Delete/{id}
        [HttpDelete]
        public async Task<IActionResult> Delete(long roleId)
        {
            _context.Remove(new CompanyRole() { Id = roleId });
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
