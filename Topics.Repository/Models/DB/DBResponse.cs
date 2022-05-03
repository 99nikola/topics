

namespace Topics.Repository.Models
{
    public class DBResponse<T> : DBResponse
    {
        public T Value { get; set; }
    }

    public class DBResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
