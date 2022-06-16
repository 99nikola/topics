using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Topics.Repository.Models.DB;
using Topics.Services.Implementations;
using Topics.Services.Interfaces;

namespace Topics.Models
{
    public class TopicModel
    {
        private ITopicService topicService;
        public TopicModel()
        {
            this.topicService  = new TopicService();
        }

        public HttpPostedFileBase CoverImg { get; set; }
        
        [Required(ErrorMessage = "A cover image is required"), FileExtensions(ErrorMessage = "Please upload an image file.", Extensions = ".png, .jpg, .jpeg")]
        public string CoverImgName
        {
            get
            {
                if (CoverImg != null)
                    return CoverImg.FileName;
                else
                    return String.Empty;
            }
        }

        public string CoverImgSrc { get; set; }

        public HttpPostedFileBase AvatarImg { get; set; }

        [Required(ErrorMessage = "An avatar image is required"), FileExtensions(ErrorMessage = "Please upload an image file.", Extensions = ".png, .jpg, .jpeg")]
        public string AvatarImgName
        {
            get
            {
                if (CoverImg != null)
                    return CoverImg.FileName;
                else
                    return String.Empty;
            }
        }

        public string AvatarImgSrc { get; set; }



        [Required(ErrorMessage = "Topic name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Topic title is required")]
        public string Title { get; set; }

        public string Description { get; set; }

        public bool IsMemeber(string username)
        {
            return topicService.IsMember(this, username);
        }

    }
}