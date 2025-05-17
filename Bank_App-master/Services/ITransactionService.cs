using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bank_App.Models;

namespace Bank_App.Services
{
    public interface ITransactionService
    {
        Task<Transaction> CreateTransactionAsync(int fromCardId, int toCardId, decimal amount);
        Task<List<Transaction>> GetTransactionsByUserIdAsync(int userId);
    }
}
