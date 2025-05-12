using Xunit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Bank_App.Data;
using Bank_App.Services;

namespace Bank_App.Tests.Services
{
    public class CreditCardServiceTests
    {
        private AppDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new AppDbContext(options);
        }

        [Fact]
        public async Task CreateCardAsync_CreatesValidCard()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var service = new CreditCardService(context);
            int userId = 1;

            // Act
            var card = await service.CreateCardAsync(userId);

            // Assert
            Assert.NotNull(card);
            Assert.Equal(userId, card.UserId);
            Assert.Equal(0, card.Balance);
            Assert.NotEmpty(card.CardNumber);
            Assert.True(int.TryParse(card.CVV, out int cvv) && cvv >= 100 && cvv <= 999);
            Assert.True(card.ExpirationDate > DateTime.Now);

            var savedCard = await context.CreditCards.FindAsync(card.Id);
            Assert.NotNull(savedCard);
        }
    }
}
