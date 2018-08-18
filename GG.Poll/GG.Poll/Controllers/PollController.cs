using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GG.Poll.Models;
using Microsoft.AspNetCore.Mvc;

namespace GG.Poll.Controllers
{
    public class PollController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult List()
        {
            return View(Enumerable.Empty<UniversalPoll>());
        }

        public IActionResult Detail()
        {
            return View(new UniversalPoll());
        }

        public IActionResult Create()
        {
            return View(new UniversalPoll());
        }

        public IActionResult Edit()
        {
            return View(new UniversalPoll());
        }
    }
}