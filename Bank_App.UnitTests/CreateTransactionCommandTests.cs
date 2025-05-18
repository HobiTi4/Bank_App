using Moq;
using Bank_App.AppLogic;
using Bank_App.Models;
using Xunit;
using System.Threading.Tasks;
using Bank_App.AppLogic.Command;

namespace Bank_App.UnitTests
{
    public class CreateTransactionCommandTests
    {
        [Fact]
        public async Task ExecuteAsync_DifferentCardIds_CreatesTransaction()
        {
            // Arrange
            var mockFacade = new Mock<IBankFacade>();
            var mockUi = new Mock<IConsoleUI>();

            // Налаштування введення
            mockUi.SetupSequence(ui => ui.PromptRequired(It.IsAny<string>()))
                  .Returns("1") // senderCardId
                  .Returns("2") // receiverCardId
                  .Returns("50"); // amount

            var expectedTransaction = new Transaction
            {
                Id = 1,
                SenderId = 1,
                ReceiverId = 2,
                Amount = 50m,
                Date = DateTime.UtcNow 
            };
            mockFacade.Setup(f => f.CreateTransactionAsync(1, 2, 50m)).ReturnsAsync(expectedTransaction);

            var command = new CreateTransactionCommand(mockFacade.Object, mockUi.Object);

            // Act
            await command.ExecuteAsync();

            // Assert
            mockUi.Verify(ui => ui.ShowMessage($"Transaction created with ID: {expectedTransaction.Id}"), Times.Once());
            mockFacade.Verify(f => f.CreateTransactionAsync(1, 2, 50m), Times.Once());
        }


        [Fact]
        public async Task ExecuteAsync_SameCardIds_AddsFunds()
        {
            // Arrange
            var mockFacade = new Mock<IBankFacade>();
            var mockUi = new Mock<IConsoleUI>();

            mockUi.SetupSequence(ui => ui.PromptRequired(It.IsAny<string>()))
                  .Returns("1") // senderCardId
                  .Returns("1") // receiverCardId
                  .Returns("100"); // amount

            mockFacade.Setup(f => f.CreateTransactionAsync(1, 1, 100m)).ReturnsAsync((Transaction)null);

            var command = new CreateTransactionCommand(mockFacade.Object, mockUi.Object);

            // Act
            await command.ExecuteAsync();

            // Assert
            mockUi.Verify(ui => ui.ShowMessage("Funds of 100 added to card ID 1."), Times.Once());
            mockFacade.Verify(f => f.CreateTransactionAsync(1, 1, 100m), Times.Once());
        }

        [Fact]
        public async Task ExecuteAsync_NegativeAmount_ShowsError()
        {
            // Arrange
            var mockFacade = new Mock<IBankFacade>();
            var mockUi = new Mock<IConsoleUI>();

            mockUi.SetupSequence(ui => ui.PromptRequired(It.IsAny<string>()))
                  .Returns("1") // senderCardId
                  .Returns("2") // receiverCardId
                  .Returns("-50"); // amount

            var command = new CreateTransactionCommand(mockFacade.Object, mockUi.Object);

            // Act
            await command.ExecuteAsync();

            // Assert
            mockUi.Verify(ui => ui.ShowError("Amount must be a positive number."), Times.Once());
            mockFacade.Verify(f => f.CreateTransactionAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<decimal>()), Times.Never());
        }

        [Fact]
        public async Task ExecuteAsync_InvalidSenderCardId_ShowsError()
        {
            // Arrange
            var mockFacade = new Mock<IBankFacade>();
            var mockUi = new Mock<IConsoleUI>();

            mockUi.SetupSequence(ui => ui.PromptRequired(It.IsAny<string>()))
                  .Returns("invalid") // senderCardId
                  .Returns("2") // receiverCardId 
                  .Returns("50"); // amount 

            var command = new CreateTransactionCommand(mockFacade.Object, mockUi.Object);

            // Act
            await command.ExecuteAsync();

            // Assert
            mockUi.Verify(ui => ui.ShowError("Sender card ID must be a number."), Times.Once());
            mockFacade.Verify(f => f.CreateTransactionAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<decimal>()), Times.Never());
        }

        [Fact]
        public async Task ExecuteAsync_InsufficientFunds_ShowsError()
        {
            // Arrange
            var mockFacade = new Mock<IBankFacade>();
            var mockUi = new Mock<IConsoleUI>();

            mockUi.SetupSequence(ui => ui.PromptRequired(It.IsAny<string>()))
                  .Returns("1") // senderCardId
                  .Returns("2") // receiverCardId
                  .Returns("100"); // amount

            mockFacade.Setup(f => f.CreateTransactionAsync(1, 2, 100m))
                      .ThrowsAsync(new Exception("Insufficient funds for the transaction."));

            var command = new CreateTransactionCommand(mockFacade.Object, mockUi.Object);

            // Act
            await command.ExecuteAsync();

            // Assert
            mockUi.Verify(ui => ui.ShowError("Insufficient funds for the transaction."), Times.Once());
            mockFacade.Verify(f => f.CreateTransactionAsync(1, 2, 100m), Times.Once());
        }
    }
}