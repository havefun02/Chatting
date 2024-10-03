using App.Core;
using App.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace App.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _userRepository;
        public UserService(IRepository<User> userRepository) {
            _userRepository = userRepository;
        }
        public async Task<User> CreateUser(string id, string name)
        {
            try {
                var dbContext = this._userRepository.GetDbSet();
                var user=await dbContext.FirstOrDefaultAsync(u=>u.UserId== id);
                if (user != null) {
                    throw new Exception("User existed");
                }
                await dbContext.AddAsync(new User { UserId=id,UserName=name});
                await _userRepository.SaveChangesAsync();
                var res = await dbContext.FirstOrDefaultAsync(u => u.UserId == id);
                if (res != null)
                {
                    return res;
                }
                else { throw new Exception("Internal error"); }
            }
            catch (Exception) { throw; }    
        }

        public async Task<User> GetUserById(string? id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException("User ID cannot be null or empty.", nameof(id));
            }

            try
            {
                var dbContext = this._userRepository.GetDbSet();
                var res=await dbContext.FirstOrDefaultAsync(u => u.UserId==id);
                if (res != null)
                {
                    return res;
                }
                else { throw new Exception("User does not exist"); }

            }
            catch { throw; }
        }
        public async Task<User> RenameUser(string id, string newName)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException("User ID cannot be null or empty.", nameof(id));
            }
            try
            {
                var dbContext = this._userRepository.GetDbSet();
                var res = await dbContext.FirstOrDefaultAsync(u => u.UserId == id);
                if (res != null)
                {
                    res.UserName = newName;
                    dbContext.Update(res);
                    await this._userRepository.SaveChangesAsync();
                    return res;
                }
                else { throw new Exception("User does not exist"); }
            }
            catch { throw; }
        }

    }
}
