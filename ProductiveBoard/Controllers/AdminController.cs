using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using ProductiveBoard.Data;
using ProductiveBoard.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

namespace ProductiveBoard.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public AdminController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }

            if (HttpContext.Session.GetString("isManager") == "0")
            {
                return RedirectToAction("Index", "Tasks");
            }

            await GetAuth();

            return View();
        }

        public async Task<IActionResult> ManageStatus()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }

            if (HttpContext.Session.GetString("isManager") == "0")
            {
                return RedirectToAction("Index", "Tasks");
            }

            List<Models.TaskStatus> statuses = await _context.TaskStatuses.ToListAsync();
            ViewBag.statuses = statuses;

            return View();
        }

        public async Task<IActionResult> ManageType()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }

            if (HttpContext.Session.GetString("isManager") == "0")
            {
                return RedirectToAction("Index", "Tasks");
            }

            List<TaskType> types = await _context.TaskTypes.ToListAsync();
            ViewBag.types = types;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateStatus(Models.TaskStatus status)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }

            if (HttpContext.Session.GetString("isManager") == "0")
            {
                return RedirectToAction("Index", "Tasks");
            }

            _context.Add(status);
            await _context.SaveChangesAsync();

            return RedirectToAction("ManageStatus");
        }

        [HttpPost]
        public async Task<IActionResult> CreateType(Models.TaskType type)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }

            if (HttpContext.Session.GetString("isManager") == "0")
            {
                return RedirectToAction("Index", "Tasks");
            }

            _context.Add(type);
            await _context.SaveChangesAsync();

            return RedirectToAction("ManageType");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteStatus(long Id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }

            if (HttpContext.Session.GetString("isManager") == "0")
            {
                return RedirectToAction("Index", "Tasks");
            }

            Models.TaskStatus status = new Models.TaskStatus() { Id = Id };

            if (Id == 0)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                _context.Remove(status);
                await _context.SaveChangesAsync();
                return RedirectToAction("ManageStatus");
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteType(long Id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }

            if (HttpContext.Session.GetString("isManager") == "0")
            {
                return RedirectToAction("Index", "Tasks");
            }

            Models.TaskType type = new Models.TaskType() { Id = Id };

            if (Id == 0)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                _context.Remove(type);
                await _context.SaveChangesAsync();
                return RedirectToAction("ManageType");
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateType(Models.TaskType type)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }

            if (HttpContext.Session.GetString("isManager") == "0")
            {
                return RedirectToAction("Index", "Tasks");
            }

            // await _userManager.RemoveFromRoleAsync()
            await _context.SaveChangesAsync();
            return RedirectToAction("ManageType");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatus(Models.TaskStatus status)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }

            if (HttpContext.Session.GetString("isManager") == "0")
            {
                return RedirectToAction("Index", "Tasks");
            }

            _context.Update(status);
            await _context.SaveChangesAsync();
            return RedirectToAction("ManageStatus");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUserRole(IdentityUserRole<string> UserRole)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }

            if (HttpContext.Session.GetString("isManager") == "0")
            {
                return RedirectToAction("Index", "Tasks");
            }
            
            
            _context.Update(UserRole);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
            
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
