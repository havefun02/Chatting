using App.Core;

namespace App.Models
{
    public class User:BaseTime
    {
        public string? UserId {  get; set; }   
        public string? UserName { get; set; }
        public virtual ICollection<Message>? UserMessages { get; set; }
    }
}
