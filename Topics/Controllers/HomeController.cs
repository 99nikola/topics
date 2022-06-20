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
    public class HomeController : Controller
    {
        private IPostService postService;

        public HomeController(IPostService postService)
        {
            this.postService = postService; 
        }

        [@Authorize(Roles = "basic")]
        public ActionResult Index()
        {
            Principal user = (Principal)HttpContext.User;

            ISet<string> upVotedPosts = postService.GetVotedPosts(user.Username, true);
            ViewBag.UpVotedPosts = upVotedPosts;

            ISet<string> downVotedPosts = postService.GetVotedPosts(user.Username, false);
            ViewBag.DownVotedPosts = downVotedPosts;


            return View(new PostList() { Posts = postService.GetAllPost() });
        }
    }
}