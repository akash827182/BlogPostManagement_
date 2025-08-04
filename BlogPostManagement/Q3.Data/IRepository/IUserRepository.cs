using Q3.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q3.Data.IRepository
{
    public interface IUserRepository
    {
        Task<User?> GetByUsernameAsync(string Username);
        Task AddAsync(User user);
        Task<IEnumerable<User>> GetAllUsersAsync();
    }
}
