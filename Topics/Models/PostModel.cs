using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Topics.Models
{
    public class PostList
    {
        public List<PostModel> Posts { get; set; }
    }

    public class PostModel
    {
        public int Id { get; set; }

        public string Username { get; set; }

        [Required(ErrorMessage = "Slug is required")]
        public string Slug { get; set; }

        public TopicModel Topic { get; set; }

        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }


        [Required(ErrorMessage = "Title is required")]
        public string Content { get; set; }

        public int UpVotes { get; set; }
        public int DownVotes { get; set; }
        public DateTime DateCreated { get; set; }

        public string TopicName { get; set; }
    }
}