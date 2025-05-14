using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bank_App.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;


namespace Bank_App.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<CreditCard> CreditCards { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public AppDbContext()
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                base.OnConfiguring(optionsBuilder);

                string connStr = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=BankAppDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";
                optionsBuilder.UseSqlServer(connStr);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // USER -> CREDITCARD (1:N)
            modelBuilder.Entity<User>()
                .HasMany(u => u.CreditCards)
                .WithOne(c => c.User)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // CREDITCARD -> TRANSACTION (Sent)
            modelBuilder.Entity<CreditCard>()
                .HasMany(c => c.SentTransactions)
                .WithOne(t => t.Sender)
                .HasForeignKey(t => t.SenderId)
                .OnDelete(DeleteBehavior.NoAction);

            // CREDITCARD -> TRANSACTION (Received)
            modelBuilder.Entity<CreditCard>()
                .HasMany(c => c.ReceivedTransactions)
                .WithOne(t => t.Receiver)
                .HasForeignKey(t => t.ReceiverId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<CreditCard>()
                .HasIndex(c => c.CardNumber)
                .IsUnique();

            modelBuilder.Entity<CreditCard>()
                .Property(c => c.Balance)
                .HasPrecision(18, 2); 

            modelBuilder.Entity<Transaction>()
                .Property(t => t.Amount)
                .HasPrecision(18, 2);  
        }

    }
}
