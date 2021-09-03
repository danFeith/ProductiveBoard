using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using ProductiveBoard.Data;
using ProductiveBoard.Models;

namespace ProductiveBoard.Controllers
{
    public class TaskTypesController : Controller
    {
        private readonly ILogger<TaskTypesController> _logger;
        private readonly ApplicationDbContext _context;

        public TaskTypesController(ILogger<TaskTypesController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        // GET /TaskTypes
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _context.TaskTypes.ToListAsync());
        }

        // GET /TaskTypes/Details/{id}
        [HttpGet]
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TaskType taskType = await _context.TaskTypes.FirstOrDefaultAsync(e => e.Id == id);
            if (taskType == null)
            {
                return NotFound();
            }

            return View(taskType);
        }

        // POST /TaskTypes
        [HttpPost]
        public async Task<IActionResult> Create(TaskType taskType)
        {
            _context.Add(taskType);
            await _context.SaveChangesAsync();
            return View(taskType);
        }

        // PUT /TaskTypes/Update
        [HttpPut]
        public async Task<IActionResult> Update(TaskType taskType)
        {
            _context.Update(taskType);
            await _context.SaveChangesAsync();
            return View(taskType);
        }

        // DELETE /TaskTypes/Delete/{id}
        [HttpDelete]
        public async Task<IActionResult> Delete(long taskTypeId)
        {
            _context.Remove(new TaskType() { Id = taskTypeId });
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
