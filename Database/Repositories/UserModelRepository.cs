using GreenThumb.Models;
using Microsoft.EntityFrameworkCore;

namespace GreenThumb.Database.Repositories
{
    public class UserModelRepository
    {
        private AppDbContext _context;

        public UserModelRepository(AppDbContext context)
        {
            _context = context;
        }

        async public Task<UserModel?> GetUserByUsernameAsync(string username)
        {
            return await _context.Users.
                FirstOrDefaultAsync(u => u.Username == username);
        }

        async public Task AddUserAsync(UserModel newUser)
        {
            await _context.Users.
                AddAsync(newUser);
        }


        async public Task<UserModel?> SignInUser(string username, string password)
        {
            //Include garden so we can add plants to the user's personal garden later on
            return await _context.Users.
                Include(u => u.Garden).
                FirstOrDefaultAsync(u => u.Username == username && u.Password == password);
        }
    }
}
