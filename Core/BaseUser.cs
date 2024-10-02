using System.Security.Claims;

namespace App.Core
{
    public class BaseUser:BaseEntity
    {
        public  virtual string? UserEmail { set; get; }
        public  virtual int UserId { set; get; }
        public  virtual int RoleId { set; get; }

    }
}
