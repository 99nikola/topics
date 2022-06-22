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
        public ActionResult Create(ModalPostModel model)
        {
            Principal user = (Principal)HttpContext.User;

            postService.CreatePost(model.Post.TopicName, user.Username, model.Post);
            return RedirectToAction("Get/" + model.Post.TopicName, "Topic", null);
        }

        [HttpPost]
        public ActionResult Edit(ModalPostModel model)
        {
            Principal user = (Principal)HttpContext.User;
            PostModel post = postService.GetPost(model.Post.Slug);

            if (user.Username.Equals(post.Username))
                postService.EditPost(model.Post);

            return RedirectToAction("Get/" + model.Post.TopicName, "Topic", null);
        }

        [HttpGet]
        public ActionResult UpVote(string id)
        {
            Principal user = (Principal)(HttpContext.User);
            postService.Vote(user.Username, id, true);
            return Redirect(Request.UrlReferrer.ToString());
        }

        [HttpGet]
        public ActionResult DownVote(string id)
        {
            Principal user = (Principal)(HttpContext.User);
            postService.Vote(user.Username, id, false);
            return Redirect(Request.UrlReferrer.ToString());
        }

        [HttpGet]
        public ActionResult View(string id)
        {
            Principal user = (Principal)HttpContext.User;

            ViewBag.UpVoted = postService.IsVotedPost(user.Username, id, true);
            ViewBag.DownVoted = postService.IsVotedPost(user.Username, id, false);

            PostModel post = postService.GetPost(id);

            return View(post);
        }
    }
}