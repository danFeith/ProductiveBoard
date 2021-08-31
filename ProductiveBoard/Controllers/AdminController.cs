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

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
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

            List<IdentityUser> users = await _context.Users.ToListAsync();
            ViewBag.users = users;

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

            _context.Update(type);
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

    }
}
