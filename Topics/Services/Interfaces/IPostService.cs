using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topics.Models;

namespace Topics.Services.Interfaces
{
    public interface IPostService
    {
        bool CreatePost(string topicName, string username, Post post);
        List<Post> GetAllPost();
        List<Post> GetTopicPosts(string topicName);
        bool Vote(string username, string postSlug, bool type);
        ISet<string> GetVotedPosts(string username, bool type);
        bool IsVotedPost(string username, string postSlug, bool type);
        Post GetPost(string postSlug);
    }
}
