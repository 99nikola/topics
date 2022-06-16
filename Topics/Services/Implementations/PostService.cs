using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Topics.Models;
using Topics.Services.Interfaces;

namespace Topics.Services.Implementations
{
    public class PostService : IPostService
    {
        private string ConnectionString
        {
            get => System.
                    Configuration.
                    ConfigurationManager.
                    ConnectionStrings["TopicsDB"].
                    ConnectionString;
        }

        public bool CreatePost(string topicName, string username, Post post)
        {
            using(SqlConnection connection = new SqlConnection())
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
                            Post (slug, username, topicName, title, content) 
                            VALUES (@slug, @username, @topicName, @title, @content)
                            ;"
                    };

                    insert.Parameters.AddWithValue("username", username);
                    insert.Parameters.AddWithValue("topicName", topicName);
                    insert.Parameters.AddWithValue("title", post.Title);
                    insert.Parameters.AddWithValue("content", post.Content);
                    insert.Parameters.AddWithValue("slug", post.Slug);


                    int rowsAffected = insert.ExecuteNonQuery();

                    connection.Close();

                    return rowsAffected == 1;
                } catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return false;
                }
            }    
        }
    }
}