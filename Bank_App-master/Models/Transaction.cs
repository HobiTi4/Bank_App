using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank_App.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public required decimal Amount { get; set; }
        public required DateTime Date { get; set; }

        public required int SenderId { get; set; }
        public virtual CreditCard? Sender { get; set; }

        public required int ReceiverId { get; set; }
        public virtual CreditCard? Receiver { get; set; }
    }

}
