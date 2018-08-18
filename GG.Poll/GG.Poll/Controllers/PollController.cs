using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GG.Poll.BizDomains;
using GG.Poll.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace GG.Poll.Controllers
{
    public class PollController : Controller
    {
        public readonly IPollBizDomain pollBiz;
        private readonly IDistributedCache cache;
        string username_key = "username";

        public PollController(IPollBizDomain pollBiz, IDistributedCache cache)
        {
            this.pollBiz = pollBiz;
            this.cache = cache;
        }

        public IActionResult Index()
        {
            var username = cache.GetString(username_key);
            if (!string.IsNullOrEmpty(username))
            {
                TempData["message"] = "Please login.";
                return RedirectToAction(nameof(List));
            }

            return View();
        }

        public IActionResult Login(string username)
        {
            cache.SetString(username_key, username);
            return RedirectToAction(nameof(List));
        }

        public IActionResult Logout()
        {
            cache.Remove(username_key);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult List()
        {
            var username = cache.GetString(username_key);
            if (string.IsNullOrEmpty(username))
            {
                TempData["message"] = "Please login.";
                return RedirectToAction(nameof(Index));
            }

            var polls = pollBiz.ListAllPoll();
            ViewBag.username = username;
            return View(polls);
        }

        public IActionResult Details(string id)
        {
            //var username = "myname";
            var username = cache.GetString(username_key);
            if (string.IsNullOrEmpty(username))
            {
                TempData["message"] = "Please login.";
                return RedirectToAction(nameof(Index));
            }

            var poll = pollBiz.GetPoll(id);
            ViewBag.username = username;
            return View(poll);
        }

        public IActionResult Vote(string pollid, string choiceid, int rating)
        {
            //var username = "myname";
            var username = cache.GetString(username_key);
            if (string.IsNullOrEmpty(username))
            {
                TempData["message"] = "Please login.";
                return RedirectToAction(nameof(Index));
            }

            pollBiz.Vote(username, pollid, choiceid, rating);

            return RedirectToAction(nameof(Details), new { id = pollid });
        }

        public IActionResult Create()
        {
            //var username = "myname";
            var username = cache.GetString(username_key);
            if (string.IsNullOrEmpty(username))
            {
                TempData["message"] = "Please login.";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.username = username;
            return View();
        }

        [HttpPost]
        public IActionResult Create(UniversalPoll poll)
        {
            //var username = "myname";
            var username = cache.GetString(username_key);
            if (string.IsNullOrEmpty(username))
            {
                TempData["message"] = "Please login.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                poll.Id = Guid.NewGuid().ToString();
                poll.Choices = poll.Choices.Where(c => !string.IsNullOrWhiteSpace(c.Title)).ToList();
                foreach (var choice in poll.Choices)
                {
                    choice.Id = Guid.NewGuid().ToString();
                    choice.Voters = Enumerable.Empty<Voter>();
                }
                poll.CreateDate = DateTime.UtcNow;
                poll.Owner = username;
                pollBiz.Create(poll);
                TempData["message"] = "Add success";
            }
            catch (Exception ex)
            {
                TempData["message"] = ex.Message;
                return View();
            }
            return RedirectToAction(nameof(List));
        }

        public IActionResult Edit(string id)
        {
            //var username = "myname";
            var username = cache.GetString(username_key);
            if (string.IsNullOrEmpty(username))
            {
                TempData["message"] = "Please login.";
                return RedirectToAction(nameof(Index));
            }

            var poll = pollBiz.GetPoll(id);
            ViewBag.username = username;
            return View(poll);
        }
    }
}