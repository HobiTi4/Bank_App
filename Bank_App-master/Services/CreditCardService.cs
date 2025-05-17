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
    public class CreditCardService : ICreditCardService
    {
        private readonly AppDbContext _context;
        private readonly ICreditCardFactory _cardFactory;
        public CreditCardService(AppDbContext context, ICreditCardFactory cardFactory)
        {
            _context = context;
            _cardFactory = cardFactory;
        }
        public async Task<CreditCard> CreateCardAsync(int userId)
        {
            var card = _cardFactory.CreateCard(userId);
            _context.CreditCards.Add(card);
            await _context.SaveChangesAsync();
            return card;
        }

        public async Task<List<CreditCard>> GetCardsByUserIdAsync(int userId)
        {
            return await _context.CreditCards
                .Where(c => c.UserId == userId)
                .ToListAsync();
        }
        public async Task<decimal> GetBalanceAsync(int cardId)
        {
            var card = await _context.CreditCards.FindAsync(cardId);
            if (card == null)
                throw new Exception("Card not found.");
            return card.Balance;
        }

        public async Task<CreditCard> GetCardByIdAsync(int cardId)
        {
            return await _context.CreditCards.FindAsync(cardId);
        }
    }
}
