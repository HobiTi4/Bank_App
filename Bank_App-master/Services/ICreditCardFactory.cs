using Bank_App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank_App.Services
{
    public interface ICreditCardFactory
    {
        CreditCard CreateCard(int userId);
    }
}
