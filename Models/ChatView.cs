using App.Core;

namespace App.Models
{
    public class ChatView
    {
        public List<MessageResultDto>? MessageResult { get; set; }
        public  User? User { get; set; }
    }
}
