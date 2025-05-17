using Bank_App.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bank_App.Models;
using Microsoft.EntityFrameworkCore;

namespace Bank_App.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly AppDbContext _context;
        public TransactionService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Transaction> CreateTransactionAsync(int fromCardId, int toCardId, decimal amount)
        {
            var fromCard = await _context.CreditCards.FindAsync(fromCardId);
            var toCard = await _context.CreditCards.FindAsync(toCardId);

            fromCard.Balance -= amount;
            toCard.Balance += amount;

            var transaction = new Transaction
            {
                SenderId = fromCardId,
                ReceiverId = toCardId,
                Amount = amount,
                Date = DateTime.Now
            };
            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();
            return transaction;
        }
        public async Task<List<Transaction>> GetTransactionsByUserIdAsync(int userId)
        {
            var userCards = await _context.CreditCards
                .Where(c => c.UserId == userId)
                .Select(c => c.Id)
                .ToListAsync();

            return await _context.Transactions
                .Where(t => userCards.Contains(t.SenderId) || userCards.Contains(t.ReceiverId))
                .ToListAsync();
        }
    }
}
