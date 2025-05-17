using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bank_App.Models;

namespace Bank_App.Services
{
    public class CreditCardFactory : ICreditCardFactory
    {
        private readonly Random _random = new Random();
        private long _lastCardNumber = 0; 
        public CreditCard CreateCard(int userId)
        {
            return new CreditCard
            {
                UserId = userId,
                CardNumber = GenerateCardNumber(),
                ExpirationDate = DateTime.Now.AddYears(3),
                CVV = GenerateCVV(),
                Balance = 0
            };
        }

        private string GenerateCardNumber()
        {
            string prefix = "4441";
            string digits = prefix + string.Join("", Enumerable.Range(0, 11).Select(_ => _random.Next(0, 10)));

            int checksum = CalculateLuhnChecksum(digits);
            return digits + checksum;
        }

        private int CalculateLuhnChecksum(string number)
        {
            int sum = 0;
            bool isEven = false;

            for (int i = number.Length - 1; i >= 0; i--)
            {
                int digit = int.Parse(number[i].ToString());
                if (isEven)
                {
                    digit *= 2;
                    if (digit > 9)
                        digit -= 9;
                }
                sum += digit;
                isEven = !isEven;
            }

            return (10 - (sum % 10)) % 10;
        }

        private string GenerateCVV()
        {
            Random random = new Random();
            return string.Join("", Enumerable.Range(0, 3).Select(_ => random.Next(0, 10)));
        }
    }
}
