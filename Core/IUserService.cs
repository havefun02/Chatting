using App.Models;
namespace App.Core
{
    public interface IUserService
    {
        public Task<User> GetUserById(string id);
        public Task<User> CreateUser(string id,string name);
        public Task<User> RenameUser(string id, string name);


    }
}
