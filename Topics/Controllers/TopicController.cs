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

        public TopicController(ITopicService topicService)
        {
            this.topicService = topicService;   
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
            if (topic == null) return View(topic);

            Principal user = (Principal)HttpContext.User;

            bool isMember = topicService.IsMember(topic, user.Username);
            ViewBag.IsMember = isMember;
            return View(topic);
        }

        [HttpPost]
        public ActionResult Create(TopicModel model)
        {
            if (!ModelState.IsValid)
                return Redirect("/");

            Principal user = (Principal)HttpContext.User;

            topicService.CreateTopic(model, user.Username);
            return Redirect("/");
        }

        [HttpGet]
        public ActionResult Join(string id)
        {
            Principal user = (Principal)HttpContext.User;
            TopicModel topic = topicService.GetTopic(id);

            topicService.AddMember(topic, user.Username);
            return RedirectToAction("Index/" + id);
        }

        [HttpGet]
        public ActionResult Leave(string id)
        {
            Principal user = (Principal)HttpContext.User;
            TopicModel topic = topicService.GetTopic(id);

            topicService.DeleteMember(topic, user.Username);
            return RedirectToAction("Index/" + id);
        }
        
    }

}
