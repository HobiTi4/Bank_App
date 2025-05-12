using Bank_App.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bank_App.Models;

namespace Bank_App.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly AppDbContext _context;
        public TransactionService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Transaction> CreateTransactionAsync(int fromUserId, int toUserId, decimal amount)
        {
            var transaction = new Transaction
            {
                SenderId = fromUserId,
                ReceiverId = toUserId,
                Amount = amount,
                Date = DateTime.Now
            };
            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();
            return transaction;
        }
    }
}
