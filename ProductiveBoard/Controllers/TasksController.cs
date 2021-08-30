using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
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
            List<TaskType> taskTypes = await _context.TaskTypes.ToListAsync();
            List<Models.TaskStatus> taskStatuses = await _context.TaskStatuses.ToListAsync();
            List<Microsoft.AspNetCore.Identity.IdentityUser> users = await _context.Users.ToListAsync();
            List<Models.Task> tasks = await _context.Tasks.ToListAsync();
            for (int currTaskIndex = 0; currTaskIndex < tasks.Count; currTaskIndex++)
            {
                for (int currUserIndex = 0; currUserIndex < taskTypes.Count; currUserIndex++)
                {
                    if (tasks[currTaskIndex].UserId == users[currUserIndex].Id)
                    {
                        tasks[currTaskIndex].User = new User();
                        tasks[currTaskIndex].User.Id = users[currUserIndex].Id;
                        tasks[currTaskIndex].User.UserName = users[currUserIndex].UserName;
                    }
                }
            }

            ViewBag.tasks = tasks;
            return View();
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
            return RedirectToAction(nameof(Index));
        }

        // PUT /Tasks/Update
        [HttpPost]
        public async Task<IActionResult> Update(Models.Task task)
        {
            _context.Update(task);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // DELETE /Tasks/Delete/{id}
        [HttpPost]
        public async Task<IActionResult> Delete(long Id)
        {
            Models.Task task = new Models.Task() { Id = Id };

            if (Id == 0)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                _context.Remove(task);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
