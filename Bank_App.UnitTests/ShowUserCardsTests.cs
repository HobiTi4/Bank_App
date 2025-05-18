using Moq;
using Bank_App.AppLogic;
using Bank_App.Models;
using Xunit;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using Bank_App.AppLogic.Command;

namespace Bank_App.UnitTests
{
    public class ShowUserCardsCommandTests
    {
        public ShowUserCardsCommandTests()
        {
            var culture = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        [Fact]
        public async Task ExecuteAsync_ValidUserIdWithCards_ShowsCards()
        {
            // Arrange
            var mockFacade = new Mock<IBankFacade>();
            var mockUi = new Mock<IConsoleUI>();

            mockUi.Setup(ui => ui.PromptRequired("Enter user ID: ")).Returns("1");

            var cards = new List<CreditCard>
            {
                new CreditCard
                {
                    Id = 1,
                    UserId = 1,
                    CardNumber = "4441234567890123",
                    Balance = 500.50m,
                    ExpirationDate = new DateTime(2026, 5, 18),
                    CVV = "123"
                },
                new CreditCard
                {
                    Id = 2,
                    UserId = 1,
                    CardNumber = "4441987654321098",
                    Balance = 1000.00m,
                    ExpirationDate = new DateTime(2026, 5, 18),
                    CVV = "456"
                }
            };
            mockFacade.Setup(f => f.GetCardsByUserIdAsync(1)).ReturnsAsync(cards);

            var command = new ShowUserCardsCommand(mockFacade.Object, mockUi.Object);

            // Act
            await command.ExecuteAsync();

            // Assert
            mockUi.Verify(ui => ui.ShowMessage("Cards for user with ID 1:"), Times.Once());
            mockUi.Verify(ui => ui.ShowMessage("Card ID: 1, Card Number: 4441234567890123, Balance: 500.50"), Times.Once());
            mockUi.Verify(ui => ui.ShowMessage("Card ID: 2, Card Number: 4441987654321098, Balance: 1000.00"), Times.Once());
            mockFacade.Verify(f => f.GetCardsByUserIdAsync(1), Times.Once());
        }

        [Fact]
        public async Task ExecuteAsync_ValidUserIdNoCards_ShowsNoCardsMessage()
        {
            // Arrange
            var mockFacade = new Mock<IBankFacade>();
            var mockUi = new Mock<IConsoleUI>();

            mockUi.Setup(ui => ui.PromptRequired("Enter user ID: ")).Returns("2");
            mockFacade.Setup(f => f.GetCardsByUserIdAsync(2)).ReturnsAsync(new List<CreditCard>());

            var command = new ShowUserCardsCommand(mockFacade.Object, mockUi.Object);

            // Act
            await command.ExecuteAsync();

            // Assert
            mockUi.Verify(ui => ui.ShowMessage("No cards found for user with ID 2."), Times.Once());
            mockFacade.Verify(f => f.GetCardsByUserIdAsync(2), Times.Once());
            mockUi.Verify(ui => ui.ShowMessage(It.Is<string>(msg => msg.StartsWith("Cards for user with ID"))), Times.Never());
        }

        [Fact]
        public async Task ExecuteAsync_InvalidUserId_ShowsError()
        {
            // Arrange
            var mockFacade = new Mock<IBankFacade>();
            var mockUi = new Mock<IConsoleUI>();

            mockUi.Setup(ui => ui.PromptRequired("Enter user ID: ")).Returns("invalid");

            var command = new ShowUserCardsCommand(mockFacade.Object, mockUi.Object);

            // Act
            await command.ExecuteAsync();

            // Assert
            mockUi.Verify(ui => ui.ShowError("ID must be a number."), Times.Once());
            mockFacade.Verify(f => f.GetCardsByUserIdAsync(It.IsAny<int>()), Times.Never());
            mockUi.Verify(ui => ui.ShowMessage(It.IsAny<string>()), Times.Never());
        }

        [Fact]
        public async Task ExecuteAsync_UserNotFound_ShowsError()
        {
            // Arrange
            var mockFacade = new Mock<IBankFacade>();
            var mockUi = new Mock<IConsoleUI>();

            mockUi.Setup(ui => ui.PromptRequired("Enter user ID: ")).Returns("3");
            mockFacade.Setup(f => f.GetCardsByUserIdAsync(3))
                      .ThrowsAsync(new Exception("User with this ID not found."));

            var command = new ShowUserCardsCommand(mockFacade.Object, mockUi.Object);

            // Act
            await command.ExecuteAsync();

            // Assert
            mockUi.Verify(ui => ui.ShowError(It.Is<string>(msg => msg.Contains("User with this ID not found"))), Times.Once());
            mockFacade.Verify(f => f.GetCardsByUserIdAsync(3), Times.Once());
            mockUi.Verify(ui => ui.ShowMessage(It.IsAny<string>()), Times.Never());
        }
    }
}