using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bank_App.Models;
using Bank_App.Data;
using Microsoft.EntityFrameworkCore;

namespace Bank_App.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        public UserService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<User> CreateUserAsync(string name, string surname, string email, string password, string? phone = null)
        {
            var user = new User
            {
                Name = name,
                Surname = surname,
                Email = email,
                Password = password,
                PhoneNumber = phone
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }
        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _context.Users
                .Include(u => u.CreditCards)
                .ToListAsync();
        }
    }
}
