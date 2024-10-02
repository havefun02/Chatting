
namespace App.Models
{
    public class MessageDto
    {
        public string? MessageId { get; set; }
        public string? UserId { get; set; }
        public string? Content { get; set; }
    }
    public class MessageResultDto
    {
        public string? UserName { get; set; }
        public string? Content { get; set; }
        public string? CreatedAt { get; set; }
    }

}
