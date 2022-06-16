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
    }
}
