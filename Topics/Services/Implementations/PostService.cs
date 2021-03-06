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

        public bool CreatePost(string topicName, string username, PostModel post)
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
                }
                catch (Exception ex)
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
                        Connection = connection,
                        CommandText = @"
                            SELECT vType 
                            FROM [VotePost] 
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
                                FROM [VotePost] 
                                WHERE username = @username AND postSlug = @slug
                                ;";

                            int rowsDeleted = command.ExecuteNonQuery();

                            connection.Close();
                            return rowsDeleted == 1;
                        }

                        command.CommandText = @"
                            UPDATE [VotePost] 
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
                        [VotePost] (username, postSlug, vType) 
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

        public List<PostModel> GetAllPost()
        {
            using (SqlConnection connection = new SqlConnection())
            {
                try
                {
                    connection.ConnectionString = ConnectionString;
                    connection.Open();

                    SqlCommand select = new SqlCommand()
                    {
                        Connection = connection,
                        CommandText = @"
                            SELECT slug, username, topicName, title, upVotes, downVotes, dateCreated, content, id
                            FROM [Post]
                            ;"
                    };

                    SqlDataReader reader = select.ExecuteReader();
                    List<PostModel> posts = new List<PostModel>();

                    while (reader.Read())
                    {
                        posts.Add(new PostModel()
                        {
                            Slug = reader.GetString(0),
                            Username = reader.GetString(1),
                            TopicName = reader.GetString(2),
                            Title = reader.GetString(3),
                            UpVotes = reader.GetInt32(4),
                            DownVotes = reader.GetInt32(5),
                            DateCreated = reader.GetDateTime(6),
                            Content = reader.GetString(7),
                            Id = reader.GetInt32(8)
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

        public ISet<string> GetVotedPosts(string username, bool type)
        {
            using (SqlConnection connection = new SqlConnection())
            {
                try
                {
                    connection.ConnectionString = ConnectionString;
                    connection.Open();

                    SqlCommand select = new SqlCommand()
                    {
                        Connection = connection,
                        CommandText = @"
                            SELECT postSlug 
                            FROM [VotePost] 
                            WHERE username = @username AND vType = @type
                            "
                    };

                    select.Parameters.AddWithValue("username", username);
                    select.Parameters.AddWithValue("type", type);

                    SqlDataReader reader = select.ExecuteReader();

                    ISet<string> posts = new HashSet<string>();

                    while (reader.Read())
                    {
                        posts.Add(reader.GetString(0));
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

        public PostModel GetPost(string postSlug)
        {
            using (SqlConnection connection = new SqlConnection())
            {
                try
                {
                    connection.ConnectionString = ConnectionString;
                    connection.Open();

                    SqlCommand select = new SqlCommand()
                    {
                        Connection = connection,
                        CommandText = @"
                            SELECT slug, username, topicName, title, upVotes, downVotes, dateCreated, content, id
                            FROM [Post]
                            WHERE slug = @slug
                            ;"
                    };

                    select.Parameters.AddWithValue("slug", postSlug);

                    SqlDataReader reader = select.ExecuteReader();

                    if (!reader.Read())
                        return null;

                    PostModel post = new PostModel()
                    {
                        Slug = reader.GetString(0),
                        Username = reader.GetString(1),
                        TopicName = reader.GetString(2),
                        Title = reader.GetString(3),
                        UpVotes = reader.GetInt32(4),
                        DownVotes = reader.GetInt32(5),
                        DateCreated = reader.GetDateTime(6),
                        Content = reader.GetString(7),
                        Id = reader.GetInt32(8),
                    };

                    connection.Close();

                    return post;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return null;
                }
            }
        }

        public bool IsVotedPost(string username, string postSlug, bool type)
        {
            using (SqlConnection connection = new SqlConnection())
            {
                try
                {
                    connection.ConnectionString = ConnectionString;
                    connection.Open();

                    SqlCommand select = new SqlCommand()
                    {
                        Connection = connection,
                        CommandText = @"
                            SELECT postSlug 
                            FROM [VotePost] 
                            WHERE username = @username AND vType = @type AND postSlug = @slug
                            ;"
                    };

                    select.Parameters.AddWithValue("username", username);
                    select.Parameters.AddWithValue("type", type);
                    select.Parameters.AddWithValue("slug", postSlug);

                    SqlDataReader reader = select.ExecuteReader();

                    if (reader.Read()) return true;
                    return false;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return false;
                }
            }
        }

        public List<PostModel> GetTopicPosts(string topicName)
        {
            using(SqlConnection connection = new SqlConnection())
            {
                try
                {
                    connection.ConnectionString = ConnectionString;
                    connection.Open();

                    SqlCommand select = new SqlCommand()
                    {
                        Connection = connection,
                        CommandText = @"
                            SELECT slug, username, topicName, title, upVotes, downVotes, dateCreated, content, id
                            FROM [Post]
                            WHERE topicName = @topicName
                            ;"
                    };
                    select.Parameters.AddWithValue("topicName", topicName);
                    SqlDataReader reader = select.ExecuteReader();

                    List<PostModel> posts = new List<PostModel>();

                    while(reader.Read())
                    {
                        posts.Add(new PostModel()
                        {
                            Slug = reader.GetString(0),
                            Username = reader.GetString(1),
                            TopicName = reader.GetString(2),
                            Title = reader.GetString(3),
                            UpVotes = reader.GetInt32(4),
                            DownVotes = reader.GetInt32(5),
                            DateCreated = reader.GetDateTime(6),
                            Content = reader.GetString(7),
                            Id = reader.GetInt32(8)
                        });
                    }

                    connection.Close();

                    return posts;
                } 
                catch(Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return null;
                }
            }
        }

        public bool EditPost(PostModel post)
        {
            using (SqlConnection connection = new SqlConnection())
            {
                try
                {
                    connection.ConnectionString = ConnectionString;
                    connection.Open();

                    SqlCommand update = new SqlCommand()
                    {
                        Connection = connection,
                        CommandText = @"
                            UPDATE [Post]
                            SET 
                                slug = @slug, 
                                title = @title, 
                                content = @content
                            WHERE id = @id
                            ;"
                    };

                    update.Parameters.AddWithValue("id", post.Id);
                    update.Parameters.AddWithValue("title", post.Title);
                    update.Parameters.AddWithValue("content", post.Content);
                    update.Parameters.AddWithValue("slug", post.Slug);


                    int rowsAffected = update.ExecuteNonQuery();

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
    }
}