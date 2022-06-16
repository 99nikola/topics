using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Topics.Auth;
using Topics.Models;
using Topics.Services.Interfaces;

namespace Topics.Controllers
{
    [@Authorize(Roles = "basic")]
    public class PostController : Controller
    {

        private readonly IPostService postService;

        public PostController(IPostService postService)
        {
            this.postService = postService;
        }

        // GET: Post
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(string id, Post post)
        {
            Principal user = (Principal)HttpContext.User;

            postService.CreatePost(id, user.Username, post);
            return RedirectToAction("Index");
        }
    }
}