using System.Collections.Generic;
using System.Web.Mvc;
using Topics.Auth;
using Topics.Models;
using Topics.Services.Interfaces;

namespace Topics.Controllers
{
    [@Authorize(Roles = "basic")]
    public class TopicController : Controller
    {
        private readonly ITopicService topicService;
        private readonly IPostService postService;

        public TopicController(ITopicService topicService, IPostService postService)
        {
            this.topicService = topicService;
            this.postService = postService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            List<TopicModel> topics = topicService.GetTopics();
            ViewBag.Topics = topics;
            return View();
        }

        [HttpGet]
        public ActionResult Get(string id)
        {
            TopicModel topic = topicService.GetTopic(id);
            Principal user = (Principal)HttpContext.User;

            bool isMember = topicService.IsMember(topic, user.Username);
            ViewBag.IsMember = isMember;
            ViewBag.TopicName = id;


            ISet<string> upVotedPosts = postService.GetVotedPosts(user.Username, true);
            ViewBag.UpVotedPosts = upVotedPosts;

            ISet<string> downVotedPosts = postService.GetVotedPosts(user.Username, false);
            ViewBag.DownVotedPosts = downVotedPosts;

            topic.Posts = postService.GetTopicPosts(id);
            return View(topic);
        }

        [HttpPost]
        public ActionResult Create(TopicModel model)
        {
            if (!ModelState.IsValid)
                return Redirect("/");

            Principal user = (Principal)HttpContext.User;

            topicService.CreateTopic(model, user.Username);
            return Redirect(Request.UrlReferrer.ToString());
        }

        [HttpPost]
        public ActionResult Edit(TopicModel model)
        {
            if (!ModelState.IsValid)
                return Redirect("/");

            Principal user = (Principal)HttpContext.User;

            topicService.EditTopic(model, user.Username);
            return Redirect(Request.UrlReferrer.ToString());
        }

        [HttpGet]
        public ActionResult Join(string id)
        {
            Principal user = (Principal)HttpContext.User;
            TopicModel topic = topicService.GetTopic(id);

            topicService.AddMember(topic, user.Username);
            return Redirect(Request.UrlReferrer.ToString());
        }

        [HttpGet]
        public ActionResult Leave(string id)
        {
            Principal user = (Principal)HttpContext.User;
            TopicModel topic = topicService.GetTopic(id);

            topicService.DeleteMember(topic, user.Username);
            return Redirect(Request.UrlReferrer.ToString());
        }
        
    }

}
