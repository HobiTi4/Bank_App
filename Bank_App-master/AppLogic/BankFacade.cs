using Bank_App.Data;
using Bank_App.Models;
using Bank_App.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank_App.AppLogic
{
    public class BankFacade
    {
        private readonly AppDbContext _context;
        private readonly IUserService _userService;
        private readonly ICreditCardService _cardService;
        private readonly ITransactionService _transactionService;

        public BankFacade(IUserService userService, ICreditCardService cardService, ITransactionService transactionService, AppDbContext context)
        {
            _context = context;
            _userService = userService;
            _cardService = cardService;
            _transactionService = transactionService;
        }
        public async Task<User> CreateUserAsync(string name, string surname, string email, string password, string? phone = null)
        {
            var existingUser = await _userService.GetUserByEmailAsync(email);
            if (existingUser != null)
                throw new Exception("User with this email already exists.");

            var user = await _userService.CreateUserAsync(name, surname, email, password, phone);
            return user;
        }

        public async Task<CreditCard> CreateCardAsync(int userId)
        {
            var userExists = await _userService.GetUserByIdAsync(userId);
            if (userExists == null)
                throw new Exception("User with this ID not found.");

            var card = await _cardService.CreateCardAsync(userId);
            return card;
        }
        public async Task<Transaction> CreateTransactionAsync(int senderCardId, int receiverCardId, decimal amount)
        {
            var senderCard = await _cardService.GetCardByIdAsync(senderCardId);
            var receiverCard = await _cardService.GetCardByIdAsync(receiverCardId);

            if (senderCard == null || receiverCard == null)
                throw new Exception("Sender or receiver card not found.");

            if (senderCardId == receiverCardId)
            {
                await AddFundsAsync(senderCardId, amount);
                return null; 
            }

            if (senderCard.Balance < amount)
                throw new Exception("Insufficient funds for the transaction.");

            var transaction = await _transactionService.CreateTransactionAsync(senderCard.UserId, receiverCard.UserId, amount);
            senderCard.Balance -= amount;
            receiverCard.Balance += amount;
            await _context.SaveChangesAsync();
            return transaction;
        }
        public async Task AddFundsAsync(int cardId, decimal amount)
        {
            if (amount <= 0)
                throw new Exception("Amount to add must be positive.");

            var card = await _cardService.GetCardByIdAsync(cardId);
            if (card == null)
                throw new Exception("Card not found.");

            card.Balance += amount;
            await _context.SaveChangesAsync();
        }
        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _userService.GetAllUsersAsync();
        }
        public async Task<List<CreditCard>> GetCardsByUserIdAsync(int userId)
        {
            var userExists = await _userService.GetUserByIdAsync(userId);
            if (userExists == null)
                throw new Exception("User with this ID not found.");

            return await _cardService.GetCardsByUserIdAsync(userId);
        }
        public async Task<decimal> GetCardBalanceAsync(int cardId)
        {
            return await _cardService.GetBalanceAsync(cardId);
        }
        public async Task<List<Transaction>> GetUserTransactionsAsync(int userId)
        {
            var userExists = await _userService.GetUserByIdAsync(userId);
            if (userExists == null)
                throw new Exception("User with this ID not found.");

            return await _transactionService.GetTransactionsByUserIdAsync(userId);
        }
    }
}
