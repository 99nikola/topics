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
        public ActionResult Create(ModalTopicModel model)
        {
            if (!ModelState.IsValid)
                return Redirect("/");

            Principal user = (Principal)HttpContext.User;

            topicService.CreateTopic(model.Topic, user.Username);
            return Redirect(Request.UrlReferrer.ToString());
        }

        [HttpPost]
        public ActionResult Edit(ModalTopicModel model)
        {
            System.Diagnostics.Debug.WriteLine(model.Id);

            if (!ModelState.IsValid)
                return Redirect("/");

            Principal user = (Principal)HttpContext.User;

            topicService.EditTopic(model.Topic, user.Username);
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
        
        [HttpPost]
        public ActionResult AddModerator(TopicModeratorModel mod) 
        {
            if (!ModelState.IsValid)
                return Redirect(Request.UrlReferrer.ToString());

            TopicModel topic = topicService.GetTopic(mod.TopicName);
            Principal user = (Principal)HttpContext.User;

            if (!topic.Owner.Equals(user.Username))
                return Redirect(Request.UrlReferrer.ToString());

            topicService.AddModerator(mod.ModUsername, mod.TopicName);
            return Redirect(Request.UrlReferrer.ToString());
        }

        public ActionResult RemoveModerator(string username, string topicName)
        {
            
            TopicModel topic = topicService.GetTopic(topicName);
            Principal user = (Principal)HttpContext.User;

            if (!topic.Owner.Equals(user.Username))
                return Redirect(Request.UrlReferrer.ToString());

            topicService.RemoveModerator(username, topicName);

            return Redirect(Request.UrlReferrer.ToString());
        }
    }

}
