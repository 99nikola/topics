using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Topics.Models
{
    public class PostList
    {
        public List<Post> Posts { get; set; }
    }

    public class Post
    {
        public int Id { get; set; }
        public string Slug { get; set; }
        public Topic Topic { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int UpVotes { get; set; }
        public int DownVotes { get; set; }
        public DateTime DateCreated { get; set; }
    }
}