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
    public class TasksController : Controller
    {
        private readonly ILogger<TasksController> _logger;
        private readonly ApplicationDbContext _context;

        public TasksController(ILogger<TasksController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        // GET /Tasks
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Tasks.ToListAsync());
        }

        // GET /Tasks/Details/{id}
        [HttpGet]
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Models.Task task = await _context.Tasks.Include(e => e.Status).FirstOrDefaultAsync(m => m.Id == id);
            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }

        // POST /Tasks
        [HttpPost]
        public async Task<IActionResult> Create(Models.Task task)
        {
            _context.Add(task);
            await _context.SaveChangesAsync();
            return View(task);
        }

        // PUT /Tasks
        [HttpPut]
        public async Task<IActionResult> Update(Models.Task task)
        {
            _context.Update(task);
            await _context.SaveChangesAsync();
            return View(task);
        }

        // DELETE /Delete/{id}
        [HttpDelete]
        public async Task<IActionResult> Delete(long taskId)
        {
            _context.Remove(taskId);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
