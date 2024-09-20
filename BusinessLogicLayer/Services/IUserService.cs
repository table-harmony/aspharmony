using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services
{
    public interface IUserService
    {
        Task<User> GetByIdAsync(int id);
        Task<User> GetByEmailAsync(string email);
        Task<User> GetByCredentialsAsync(string email, string password);
        Task<IEnumerable<User>> GetAllAsync();
        Task CreateAsync(string email, string password);
        Task UpdateAsync(User user);
        Task DeleteAsync(int id);
    }
}
