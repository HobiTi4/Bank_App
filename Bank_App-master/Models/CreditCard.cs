using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank_App.Models
{
    public class CreditCard
    {
        public int Id { get; set; }
        public required string CardNumber { get; set; }
        public required DateTime ExpirationDate { get; set; }
        public required string CVV { get; set; }
        public required decimal Balance { get; set; }

        public required int UserId { get; set; }
        public virtual User? User { get; set; }

        public virtual ICollection<Transaction> SentTransactions { get; set; } = new List<Transaction>();
        public virtual ICollection<Transaction> ReceivedTransactions { get; set; } = new List<Transaction>();
    }

}
