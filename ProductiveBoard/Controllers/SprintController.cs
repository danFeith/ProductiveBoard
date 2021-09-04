using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProductiveBoard.Data;
using ProductiveBoard.Models;
using Task = ProductiveBoard.Models.Task;

namespace ProductiveBoard.Controllers
{
    public class SprintController : Controller
    {
        private readonly ILogger<SprintController> _logger;
        private readonly ApplicationDbContext _context;

        public SprintController(ILogger<SprintController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        // GET /Sprints
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] long? taskId)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }

            await GetAuth();
            List<Models.Sprint> sprints = await _context.sprints.Include(a => a.sprintTasks).ThenInclude(st => st.task).ToListAsync();


            ViewBag.sprints = sprints;
            bool isQuery = false;

            IEnumerable<Models.Sprint> filteredSprints = _context.sprints.Include(a => a.sprintTasks).ThenInclude(st => st.task);

            if (taskId.HasValue)
            {
                filteredSprints = filteredSprints.Where(t => t.sprintTasks.Any(st => st.taskId == taskId));
                filteredSprints = filteredSprints.ToList();
                isQuery = true;
            }

            if (isQuery)
            {
                ViewBag.tasks = filteredSprints;
            }

            return View();
        }

        // GET Sprints/Filter?taskId=X
        [HttpGet]
        public IEnumerable<Models.Sprint> Filter([FromQuery] long? taskId)
        {
            IEnumerable<Models.Sprint> sprints = _context.sprints.Include(a => a.sprintTasks)
                                                                 .ThenInclude(st => st.task);

            if (taskId.HasValue)
            {
                sprints = sprints.Where(t => t.sprintTasks.Any(st => st.taskId == taskId));
            }
            return sprints;
        }

        // GET /Sprints/Details/{id}
        [HttpGet]
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Models.Sprint sprint = await _context.sprints.FirstOrDefaultAsync(s => s.Id == id);
            if (sprint == null)
            {
                return NotFound();
            }

            return View(sprint);
        }

        // POST /Sprints
        [HttpPost]
        public async Task<IActionResult> Create(Sprint sprint)
        {
            _context.Add(sprint);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // PUT /Sprints/Update
        [HttpPost]
        public async Task<IActionResult> Update(Sprint sprint)
        {
            _context.Update(sprint);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // PUT /Sprints/AddTask
        [HttpPost]
        public async Task<IActionResult> AddTask(long? taskId, long? sprintId)
        {
            Task task = await _context.Tasks.FirstOrDefaultAsync(t=> t.Id == taskId);
            Sprint sprint = await _context.sprints.Include(s => s.sprintTasks).ThenInclude(st=> st.task).FirstOrDefaultAsync(t=> t.Id == sprintId);
            if(task == null || sprint == null)
            {
                return NotFound();
            }

            sprint.sprintTasks.Add(new SprintTask() { task = task,
                                    taskId = (long)taskId,
                                    sprint = sprint,
                                    sprintId = (long)sprintId});

            _context.Update(sprint);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // DELETE /Sprints/Delete/{id}
        [HttpPost]
        public async Task<IActionResult> Delete(long Id)
        {
            Sprint sprint = new Sprint() { Id = Id };

            if (Id == 0)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                if (_context.sprints.Include(s => s.sprintTasks)
                    .FirstOrDefault(s => s.Id == Id).sprintTasks.Count == 0)
                {
                    _context.Remove(sprint);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                return View();
            }
        }


        public async Task<List<IUser>> GetAuth()
        {
            List<IdentityRole> roles = await _context.Roles.ToListAsync();

            ViewBag.roles = roles;

            List<IdentityUserRole<string>> userRoles = await _context.UserRoles.ToListAsync();
            List<IdentityUser> users = await _context.Users.ToListAsync();
            List<IUser> extendedUsers = new List<IUser>();

            for (int currUser = 0; currUser < users.Count; currUser++)
            {
                for (int currUserRole = 0; currUserRole < userRoles.Count; currUserRole++)
                {
                    if (users[currUser].Id == userRoles[currUserRole].UserId)
                    {
                        for (int currRole = 0; currRole < roles.Count; currRole++)
                        {
                            if (roles[currRole].Id == userRoles[currUserRole].RoleId)
                            {
                                bool isManager = roles[currRole].Id == "1";

                                extendedUsers.Add(new IUser(userRoles[currUserRole].RoleId, userRoles[currUserRole].UserId, users[currUser].Email, roles[currRole].Name, isManager));
                            }
                        }
                    }
                }
            }
            for (int currUser = 0; currUser < extendedUsers.Count; currUser++)
            {
                if (User.Identity.Name == extendedUsers[currUser].Email)
                {
                    if (extendedUsers[currUser].isManager)
                    {
                        ViewBag.isManager = true;
                        HttpContext.Session.SetString("isManager", "1");
                    }
                    else
                    {
                        ViewBag.isManager = false;
                        HttpContext.Session.SetString("isManager", "0");
                    }
                }
            }

            ViewBag.users = users;
            ViewBag.extendedUsers = extendedUsers;
            return extendedUsers;
        }
    }
}
