using App.Core;
using App.Exceptions;
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
            var dbContext = this._userRepository.GetDbSet();
            var user = await dbContext.FirstOrDefaultAsync(u => u.UserId == id);
            if (user != null) {
                throw new NotFoundException("User existed");
            }
            await dbContext.AddAsync(new User { UserId = id, UserName = name });
            await _userRepository.SaveChangesAsync();
            var res = await dbContext.FirstOrDefaultAsync(u => u.UserId == id);
            if (res != null)
            {
                return res;
            }
            else { throw new UnknownException("Internal error"); }
        }

        public async Task<User> GetUserById(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException("User ID cannot be null or empty.", nameof(id));
            }

            var dbContext = this._userRepository.GetDbSet();
            var res = await dbContext.FirstOrDefaultAsync(u => u.UserId == id);
            if (res != null)
            {
                return res;
            }
            else { throw new NotFoundException("User does not exist"); }

        }
        public async Task<User> RenameUser(string id, string newName)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException("User ID cannot be null or empty.", nameof(id));
            }
            var dbContext = this._userRepository.GetDbSet();
            var res = await dbContext.FirstOrDefaultAsync(u => u.UserId == id);
            if (res != null)
            {
                res.UserName = newName;
                dbContext.Update(res);
                await this._userRepository.SaveChangesAsync();
                return res;
            }
            else { throw new UnknownException("User does not exist"); }
        }
    }
}
