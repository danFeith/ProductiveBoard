using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using ProductiveBoard.Models;

namespace ProductiveBoard.Controllers
{
    public class Task
    {
        public Task()
        {
            this.title = "";
            this.description = "";
        }

        public Task(string title, string description)
        {
            this.title = title;
            this.description = description;
        }
        public string title { get; set; }
        public string description { get; set; }
    }

    public class HomeController : Controller
    {
        
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Index()
        {
            // Read from DB All the tasks...
            List<Task> tasks = new List<Task>();
            tasks.Add(new Task("CoolOne", "Make a Cool Description in here"));
            tasks.Add(new Task("CoolTwo", "Make a Cool Description in here TOO"));
            tasks.Add(new Task("CoolThree", "Make a Cool Description in here TOO Again and again"));

            ViewBag.tasks = tasks;
            return View();
        }

        [HttpPost]
        public IActionResult Create(string title, string description)
        {
            // Add Create in db...
            return RedirectToAction("Index");
        }

        public IActionResult Statistics()
        {
            return View();
        }

        [HttpPut]
        public IActionResult Update()
        {
            // Add Update in db...
            return RedirectToAction("Index");
        }

        [HttpDelete]
        public IActionResult Delete()
        {
            // Add Delete in db...
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
