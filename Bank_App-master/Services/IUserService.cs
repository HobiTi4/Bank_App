using Bank_App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank_App.Services
{
    public interface IUserService
    {
        Task<User> CreateUserAsync(string name, string surname, string email, string password, string? phone = null);
        Task<List<User>> GetAllUsersAsync();
    }
}
