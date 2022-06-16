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

        public bool Vote(string username, string postSlug, bool type)
        {
            using (SqlConnection connection = new SqlConnection())
            {
                try
                {
                    connection.ConnectionString = ConnectionString;
                    connection.Open();

                    SqlCommand command = new SqlCommand()
                    {
                        Connection=connection,
                        CommandText = @"
                            SELECT vType 
                            FROM [Vote] 
                            WHERE username = @username AND postSlug = @slug
                            ;"
                    };

                    command.Parameters.AddWithValue("username", username);
                    command.Parameters.AddWithValue("slug", postSlug);
                    command.Parameters.AddWithValue("type", type);

                    SqlDataReader reader = command.ExecuteReader();

                    
                    if (reader.Read())
                    {
                        bool vType = reader.GetBoolean(0);
                        reader.Close();

                        if (vType == type)
                        {
                            command.CommandText = @"
                                DELETE 
                                FROM [Vote] 
                                WHERE username = @username AND postSlug = @slug
                                ;";

                            int rowsDeleted = command.ExecuteNonQuery();

                            connection.Close();
                            return rowsDeleted == 1;
                        }

                        command.CommandText = @"
                            UPDATE [Vote] 
                            SET vType = @type
                            WHERE username = @username AND postSlug = @slug
                            ;";

                        int rowsUpdated = command.ExecuteNonQuery();

                        connection.Close();
                        return rowsUpdated == 1;
                    }

                    reader.Close();

                    command.CommandText = @"
                        INSERT INTO 
                        [Vote] (username, postSlug, vType) 
                        VALUES (@username, @slug, @type)
                        ;";

                    int rowsAffected = command.ExecuteNonQuery();

                    connection.Close();
                    return (rowsAffected == 1);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return false;
                }
            }
        }

        public List<Post> GetAllPost()
        {
            using (SqlConnection connection = new SqlConnection())
            {
                try
                {
                    connection.ConnectionString = ConnectionString;
                    connection.Open();

                    SqlCommand select = new SqlCommand()
                    {
                        Connection= connection,
                        CommandText = @"
                            SELECT slug, username, topicName, title, upVotes, downVotes, dateCreated, content
                            FROM [Post]
                            ;"
                    };

                    SqlDataReader reader = select.ExecuteReader();
                    List<Post> posts = new List<Post>();

                    while(reader.Read())
                    {
                        posts.Add(new Post()
                        {
                            Slug = reader.GetString(0),
                            TopicName = reader.GetString(2),
                            Title = reader.GetString(3),
                            UpVotes = reader.GetInt32(4),
                            DownVotes = reader.GetInt32(5),
                            DateCreated = reader.GetDateTime(6),
                            Content = reader.GetString(7)
                        });
                    }
                    

                    connection.Close();
                    return posts;
                } 
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return null; 
                }
            }
        }

    }
}