using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Topics.Models
{
    public class CommentModel
    {
        public int Id { get; set; }
        public int UpVotes { get; set; }
        public int DownVotes { get; set; }
        public int ParentComment { get; set; }
        public string Username { get; set; }
        public string PostSlug { get; set; }
        public string Content { get; set; }

    }
}