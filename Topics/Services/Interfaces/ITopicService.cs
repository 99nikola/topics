using System.Collections.Generic;
using Topics.Models;

namespace Topics.Services.Interfaces
{
    public interface ITopicService
    {
        bool CreateTopic(TopicModel topic, string username);
        TopicModel GetTopic(string name);
        bool AddMember(TopicModel topic, string username); 
        bool IsMember(TopicModel topic, string username);   
        bool DeleteMember(TopicModel topic, string username);
        List<TopicModel> GetTopics();
        List<string> GetModerators(string topicName);
        bool EditTopic(TopicModel topic, string username);
        bool AddModerator(string username, string topicName);
        bool RemoveModerator(string username, string topicName);

    }
}
