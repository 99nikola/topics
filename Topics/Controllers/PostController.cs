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


        [HttpPost]
        public ActionResult Create(Post post)
        {
            Principal user = (Principal)HttpContext.User;

            postService.CreatePost(post.TopicName, user.Username, post);
            return RedirectToAction("Get/" + post.TopicName, "Topic", null);
        }

        [HttpGet]
        public ActionResult UpVote(string id)
        {
            Principal user = (Principal)(HttpContext.User);
            postService.Vote(user.Username, id, true);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult DownVote(string id)
        {
            Principal user = (Principal)(HttpContext.User);
            postService.Vote(user.Username, id, false);
            return RedirectToAction("Index", "Home");
        }
    }
}