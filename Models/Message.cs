using App.Core;
namespace App.Models
{
    public class Message:BaseTime
    {
        public string? MessageId { get; set; }
        public  string? UserId {  get; set; }
        public string? Content { get; set; }
        public virtual User? User { get; set; }  
    }
}
