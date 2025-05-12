using Bank_App.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bank_App.Models;

namespace Bank_App.Services
{
    public class CreditCardService : ICreditCardService
    {
        private readonly AppDbContext _context;
        private readonly Random _random = new Random();
        public CreditCardService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<CreditCard> CreateCardAsync(int userId)
        {
            var card = new CreditCard
            {
                UserId = userId,
                Balance = 0,
                CardNumber = GenerateCardNumber(),
                ExpirationDate = DateTime.Now.AddYears(4),
                CVV = GenerateCvv()
            };
            _context.CreditCards.Add(card);
            await _context.SaveChangesAsync();
            return card;
        }

        private string GenerateCardNumber()
        {
            var sb = new StringBuilder("4441");

            for (int i = 0; i < 12; i++) 
            {
                sb.Append(_random.Next(0, 10));
            }

            return sb.ToString();
        }

        private string GenerateCvv()
        {
            return _random.Next(100, 1000).ToString(); 
        }
    }
}
