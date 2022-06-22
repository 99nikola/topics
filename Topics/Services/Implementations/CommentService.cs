using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Topics.Models;
using Topics.Services.Interfaces;

namespace Topics.Services.Implementations
{
    public class CommentService : ICommentService
    {
        private string ConnectionString
        {
            get => System.
                    Configuration.
                    ConfigurationManager.
                    ConnectionStrings["TopicsDB"].
                    ConnectionString;
        }

        public bool CreateComment(CommentModel comment)
        {
            using (SqlConnection connection = new SqlConnection())
            {
                try
                {
                    connection.ConnectionString = ConnectionString;
                    connection.Open();

                    SqlCommand insert = new SqlCommand()
                    {
                        Connection = connection,
                        CommandText = @"
                            INSERT INTO 
                            Comment (postSlug, ownerUsername,  content) 
                            VALUES (@slug, @username, @content)
                            ;"
                    };

                    insert.Parameters.AddWithValue("username", comment.Username);
                    insert.Parameters.AddWithValue("postSlug", comment.PostSlug);
                    insert.Parameters.AddWithValue("content", comment.Content);


                    int rowsAffected = insert.ExecuteNonQuery();

                    connection.Close();

                    return rowsAffected == 1;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return false;
                }
            }
        }

        public bool DeleteComment(CommentModel comment)
        {
            throw new NotImplementedException();
        }

        public bool Edit(CommentModel comment)
        {
            throw new NotImplementedException();
        }

        public bool VoteComment(CommentModel comment, string username, bool type)
        {
            throw new NotImplementedException();
        }
    }
}