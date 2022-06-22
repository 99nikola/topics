using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topics.Models;

namespace Topics.Services.Interfaces
{
    public interface ICommentService
    {
        bool CreateComment(CommentModel comment);
        bool DeleteComment(CommentModel comment);
        bool Edit(CommentModel comment);
        bool VoteComment(CommentModel comment, string username, bool type);
    }
}
