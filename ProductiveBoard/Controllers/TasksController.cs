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
using System.IO;
using System.Text;
using System;

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
        public async Task<IActionResult> Index([FromQuery] long? typeId, 
                                               [FromQuery] long? statusId, 
                                               [FromQuery] long? sprintId)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }

            await GetAuth();
            List<IdentityUser> users = new List<IdentityUser>();
            List<IUser> extendedUsers = new List<IUser>();
            users = ViewBag.users;
            extendedUsers = ViewBag.extendedUsers;

            ViewBag.extendedUsers = extendedUsers;
            List<TaskType> taskTypes = await _context.TaskTypes.ToListAsync();
            List<Models.TaskStatus> taskStatuses = await _context.TaskStatuses.ToListAsync();
            List<Models.Task> tasks = await _context.Tasks.Include(a => a.sprintTasks).ThenInclude(st => st.sprint).ToListAsync();

            ViewBag.users = users;
            ViewBag.taskTypes = taskTypes;
            ViewBag.taskStatuses = taskStatuses;
            for (int currTaskIndex = 0; currTaskIndex < tasks.Count; currTaskIndex++)
            {
                for (int currUserIndex = 0; currUserIndex < users.Count; currUserIndex++)
                {
                    if (tasks[currTaskIndex].UserId == users[currUserIndex].Id)
                    {
                        tasks[currTaskIndex].UserId = users[currUserIndex].Id;
                        tasks[currTaskIndex].User.UserName = users[currUserIndex].UserName;
                        tasks[currTaskIndex].UserId = users[currUserIndex].Id;
                    }
                }
            }   

            ViewBag.tasks = tasks;
            bool isQuery = false;

            IEnumerable<Models.Task> filteredTasks = _context.Tasks.Include(a => a.sprintTasks).ThenInclude(st => st.sprint);

            if (typeId.HasValue)
            {
                filteredTasks = _context.Tasks.Where(t => t.TypeId == typeId);
                filteredTasks = filteredTasks.ToList();
                isQuery = true;
            }

            if (statusId.HasValue)
            {
                filteredTasks = filteredTasks.Where(t => t.StatusId == statusId);
                filteredTasks = filteredTasks.ToList();
                isQuery = true;
            }

            if (sprintId.HasValue)
            {
                filteredTasks = filteredTasks.Where(t => t.sprintTasks.Any(st => st.sprintId == sprintId));
                filteredTasks = filteredTasks.ToList();
                isQuery = true;
            }

            if (isQuery)
            {
                ViewBag.tasks = filteredTasks;
            }

            return Sprints(null).Result;
        }

        // GET Tasks/Filter?taskId=X&statusId=Y
        [HttpGet]
        public IEnumerable<Models.Task> Filter([FromQuery]long? typeId, 
                                               [FromQuery] long? statusId,
                                               [FromQuery] long? sprintId)
        {
            IEnumerable<Models.Task> tasks = _context.Tasks.Include(a => a.sprintTasks).ThenInclude(st => st.sprint); ;

            if (typeId.HasValue)
            {
                tasks = _context.Tasks.Where(t => t.TypeId == typeId);
            }

            if (statusId.HasValue)
            {
                tasks = tasks.Where(t => t.StatusId == statusId);
            }

            if (sprintId.HasValue)
            {
                tasks = tasks.Where(t => t.sprintTasks
                                .Any(st => st.sprintId == sprintId));
            }

            return tasks;
        }

        // GET Tasks/TasksCountByStatus
        [HttpGet]
        public IEnumerable TasksCountByStatus()
        {
            var statusesCount = _context.Tasks
                .GroupBy(t => new { Id = t.StatusId, Name = t.Status.Name })
                .Select(g => new { Name = g.Key.Name, Count = g.Count() })
                .ToList();

            return statusesCount;
        }

        // GET Tasks/TasksCountByUser
        [HttpGet]
        public IEnumerable TasksCountByUser()
        {
            var usersCount = _context.Tasks
                .GroupBy(t => new { Id = t.UserId, Name = t.User.UserName })
                .Select(g => new { Name = g.Key.Name, Value = g.Count() })
                .ToList();

            return usersCount;
        }

        // GET Tasks/TasksCountByType
        [HttpGet]
        public IEnumerable TasksCountByType()
        {

            var usersCount = _context.Tasks
                .GroupBy(t => new { Id = t.TypeId, Name = t.Type.Name })
                .Select(g => new { Name = g.Key.Name, Value = g.Count() })
                .ToList();

            return usersCount;
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
        public async Task<IActionResult> Create(Models.Task task, Dictionary<string, string> dataDict)
        {
            _context.Add(task);
            await _context.SaveChangesAsync();

            List<long> sprintIds = dataDict.Values.ToList().GetRange(5, dataDict.Values.ToList().Count - 6).Select(a => (long)Int32.Parse(a)).ToList();

            foreach (long sprintId in sprintIds)
            {
                Sprint sprint = await _context.sprints.Include(s => s.sprintTasks).ThenInclude(st => st.task).FirstOrDefaultAsync(t => t.Id == sprintId);
                if (task == null || sprint == null)
                {
                    return NotFound();
                }
                sprint.sprintTasks.Add(new SprintTask()
                {
                    task = task,
                    taskId = task.Id,
                    sprint = sprint,
                    sprintId = sprintId
                });
                _context.Update(sprint);
            }
            
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // PUT /Tasks/Update
        [HttpPost]
        public async Task<IActionResult> Update(Models.Task task, Dictionary<string, string> dataDict)
        {
            _context.Update(task);
            await _context.SaveChangesAsync();

            List<long> sprintIds = dataDict.Values.ToList().GetRange(6, dataDict.Values.ToList().Count - 7).Select(a => (long)Int32.Parse(a)).ToList();

            foreach (long sprintId in sprintIds)
            {
                Sprint sprint = await _context.sprints.Include(s => s.sprintTasks).ThenInclude(st => st.task).FirstOrDefaultAsync(t => t.Id == sprintId);
                if (task == null || sprint == null)
                {
                    return NotFound();
                }
                if (!sprint.sprintTasks.Any(st => st.taskId == task.Id))
                {
                    sprint.sprintTasks.Add(new SprintTask()
                    {
                        task = task,
                        taskId = task.Id,
                        sprint = sprint,
                        sprintId = sprintId
                    });
                }
                _context.Update(sprint);
            }

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

        //Sprints api for task view

        // GET /Tasks/Sprints
        [HttpGet]
        public async Task<IActionResult> Sprints([FromQuery] long? taskId)
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
                ViewBag.sprints = filteredSprints;
            }

            return View();
        }



        // POST /Tasks/AddSprint
        [HttpPost]
        public async Task<IActionResult> AddSprint()
        {
            using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
            {
                string sprintName = await reader.ReadToEndAsync();
                _context.Add(new Sprint() { name = sprintName });
                await _context.SaveChangesAsync();
            }

            return View();
        }

        // PUT /Tasks/UpdateSprint
        [HttpPost]
        public async Task<IActionResult> UpdateSprint(Sprint sprint)
        {
            _context.Update(sprint);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // PUT /Tasks/AddTaskToSprint
        [HttpPost]
        public async Task<IActionResult> AddTaskToSprint(long? taskId, long? sprintId)
        {
            Task task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == taskId);
            Sprint sprint = await _context.sprints.Include(s => s.sprintTasks).ThenInclude(st => st.task).FirstOrDefaultAsync(t => t.Id == sprintId);
            if (task == null || sprint == null)
            {
                return NotFound();
            }

            sprint.sprintTasks.Add(new SprintTask()
            {
                task = task,
                taskId = (long)taskId,
                sprint = sprint,
                sprintId = (long)sprintId
            });

            _context.Update(sprint);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // DELETE /Tasks/DeleteSprint/{id}
        [HttpPost]
        public async Task<IActionResult> DeleteSprint(long Id)
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

        public IActionResult Statistics()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }

            if (HttpContext.Session.GetString("isManager") == "0")
            {
                return RedirectToAction("Index", "Tasks");
            }

            return View();
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
