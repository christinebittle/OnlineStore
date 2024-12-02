namespace OnlineStore.Models
{
    public class Prompt
    {
        // gpt model
        public string model { get; set; }

        // messages
        public List<Message> messages { get; set; }


    }

    public class Message
    {
        // system or user
        public string role { get; set; }

        // content of message
        public string content { get; set; }
    }
}
