using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;
using Topics.Models;
using System.IO;
using Topics.Services.Interfaces;
using Topics.Repository;

namespace Topics.Services.Implementations
{
    public class TopicService : ITopicService
    {
        private string ConnectionString
        {
            get => System.
                    Configuration.
                    ConfigurationManager.
                    ConnectionStrings["TopicsDB"].
                    ConnectionString;
        }

        public bool CreateTopic(TopicModel topic, string username)
        {
            using (SqlConnection connection = new SqlConnection())
            {
                try
                {
                    connection.ConnectionString = ConnectionString;
                    connection.Open();

                    SqlCommand insert = new SqlCommand()
                    {
                        CommandText = @"
                            INSERT INTO 
                            [Topic] (name, title, description, cover, owner, avatar)
                            VALUES (@name, @title, @description, @cover, @owner, @avatar)
                            ;",
                        Connection = connection
                    };

                    insert.Parameters.AddWithValue("name", topic.Name);
                    insert.Parameters.AddWithValue("title", topic.Title);
                    insert.Parameters.AddWithValue("description", topic.Description);
                    insert.Parameters.AddWithValue("cover", ConvertToBytes(topic.CoverImg));
                    insert.Parameters.AddWithValue("owner", username);
                    insert.Parameters.AddWithValue("avatar", ConvertToBytes(topic.AvatarImg));

                    int affected = insert.ExecuteNonQuery();

                    connection.Close();

                    return affected != 0;
                } catch(Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    return false;
                }
            }

        }
        
        public TopicModel GetTopic(string name)
        {
            using(SqlConnection connection = new SqlConnection())
            {
                try
                {
                    connection.ConnectionString = ConnectionString;
                    connection.Open();

                    SqlCommand select = new SqlCommand()
                    {
                        CommandText = @"
                            SELECT name, title, description, cover, owner, avatar
                            FROM [Topic] 
                            WHERE name = @name
                            ;",
                        Connection = connection
                    };

                    select.Parameters.AddWithValue("name", name);

                    SqlDataReader reader = select.ExecuteReader();

                    if (!reader.Read()) return null;


                    TopicModel topic = new TopicModel()
                    {
                        Name = reader.GetString(0),
                        Title = reader.GetString(1),
                        Description = reader.GetString(2),  
                        CoverImgSrc = ConvertToImage(ConvertToBase64(Utils.ConvertFromDBVal<byte[]>(reader.GetValue(3)))),
                        AvatarImgSrc = ConvertToImage(ConvertToBase64(Utils.ConvertFromDBVal<byte[]>(reader.GetValue(5))))
                    };

                    connection.Close();
                    
                    return topic;
                } catch(Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    return null;
                }
            }

        }
        private byte[] ConvertToBytes(HttpPostedFileBase file)
        {
            BinaryReader rdr = new BinaryReader(file.InputStream);
            return rdr.ReadBytes(file.ContentLength);
        }

        private string ConvertToBase64(byte[] file)
        {
            if (file == null) return null;
            return Convert.ToBase64String(file);
        }

        private string ConvertToImage(string base64)
        {
            if (base64 == null) return null;
            return string.Format("data:image/gif;base64, {0}", base64);
        }

        public bool AddMember(TopicModel topic, string username)
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
                            [Topic_Member] (topicName, memberName)
                            VALUES (@topic, @member)
                            ;"
                    };

                    insert.Parameters.AddWithValue("topic", topic.Name);
                    insert.Parameters.AddWithValue("member", username);

                    int rowsAffected = insert.ExecuteNonQuery();

                    connection.Close();
                    return rowsAffected == 1;

                } catch(Exception e)
                {
                    Debug.WriteLine(e.Message);
                    return false;
                }
            }
        }

        public bool IsMember(TopicModel topic, string username)
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
                            SELECT *
                            FROM [Topic_Member] 
                            WHERE topicName = @topic AND memberName = @member
                            ;"
                    };

                    select.Parameters.AddWithValue("topic", topic.Name);
                    select.Parameters.AddWithValue("member", username);

                    SqlDataReader reader = select.ExecuteReader();
                    bool isMember = reader.Read();

                    connection.Close();
                    return isMember;

                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                    return false;
                }
            }
        }

        public bool DeleteMember(TopicModel topic, string username)
        {
            using (SqlConnection connection = new SqlConnection())
            {
                try
                {
                    connection.ConnectionString = ConnectionString;
                    connection.Open();

                    SqlCommand delete = new SqlCommand()
                    {
                        Connection = connection,
                        CommandText = @"
                            DELETE 
                            FROM [Topic_Member] 
                            WHERE topicName = @topic AND memberName = @member
                            ;"
                    };

                    delete.Parameters.AddWithValue("topic", topic.Name);
                    delete.Parameters.AddWithValue("member", username);

                    int rowsAffected = delete.ExecuteNonQuery();
                    connection.Close();
                    return rowsAffected == 1;
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                    return false;
                }
            }
        }

        public List<TopicModel> GetTopics()
        {
            using(SqlConnection connection = new SqlConnection())
            {
                try
                {
                    connection.ConnectionString= ConnectionString;
                    connection.Open();

                    SqlCommand select = new SqlCommand()
                    {
                        Connection= connection,
                        CommandText = @"
                            SELECT name, cover, title, description 
                            FROM [Topic]
                            ;"
                    };

                    SqlDataReader reader = select.ExecuteReader();

                    List<TopicModel> topics = new List<TopicModel>();

                    while(reader.Read())
                    {
                        topics.Add(new TopicModel()
                        {
                            Name = reader.GetString(0),
                            CoverImgSrc = ConvertToImage(ConvertToBase64(Utils.ConvertFromDBVal<byte[]>(reader.GetValue(1)))),
                            Title = reader.GetString(2),
                            Description = reader.GetString(3),
                        });
                    }

                    connection.Close();
                    return topics;
                    
                } catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                    return null;
                }
            }
        }
    }
}