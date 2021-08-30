﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProductiveBoard.Models;

namespace ProductiveBoard.Controllers
{
    public class UtilsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";
            LocationLists model = new LocationLists();
            var locations = new List<Locations>()
            {
                new Locations(1, "Ilan Malisov","Herzliya, israel", 32.163510, 34.842670),
                new Locations(2, "Michael Tsiriulinikov","Bat Yam, israel", 32.011850, 34.745310),
                new Locations(3, "Ofek Reuveni","Rishon Lezion, israel", 31.963760, 34.815720),
                new Locations(3, "Colman Collage of management","Rishon Lezion, israel", 31.986460, 34.806020)
            };
            model.LocationList = locations;
            return View(model);
        }
    }
}
