using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Topics.Authentication;
using Topics.Models;

namespace Topics.Controllers
{
    public class HomeController : Controller
    {
        [@Authorize(Roles = "basic")]
        public ActionResult Index()
        {
            return View(new PostList() { Posts = new List<Post>() { new Post(), new Post() } });
        }
    }
}