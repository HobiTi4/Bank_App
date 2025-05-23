﻿using Bank_App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank_App.Services
{
    public interface ICreditCardService
    {
        Task<CreditCard> CreateCardAsync(int userId);
        Task<List<CreditCard>> GetCardsByUserIdAsync(int userId);
        Task<decimal> GetBalanceAsync(int cardId);
        Task<CreditCard> GetCardByIdAsync(int cardId);

    }
}
