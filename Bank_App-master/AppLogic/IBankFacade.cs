using Bank_App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank_App.AppLogic
{
    public interface IBankFacade
    {
        Task<User> CreateUserAsync(string name, string surname, string email, string password, string? phone = null);
        Task<CreditCard> CreateCardAsync(int userId);
        Task<Transaction> CreateTransactionAsync(int senderCardId, int receiverCardId, decimal amount);
        Task AddFundsAsync(int cardId, decimal amount);
        Task<List<User>> GetAllUsersAsync();
        Task<List<CreditCard>> GetCardsByUserIdAsync(int userId);
        Task<decimal> GetCardBalanceAsync(int cardId);
        Task<List<Transaction>> GetUserTransactionsAsync(int userId);
    }
}
