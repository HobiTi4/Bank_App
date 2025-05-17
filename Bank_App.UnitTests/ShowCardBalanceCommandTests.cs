using Bank_App.AppLogic.Command;
using Bank_App.AppLogic;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Bank_App.UnitTests
{
    public class ShowCardBalanceCommandTests
    {
        [Fact]
        public async Task ExecuteAsync_ValidCardId_ShowsBalance()
        {
            // Arrange
            var mockFacade = new Mock<IBankFacade>();
            var mockUi = new Mock<IConsoleUI>();

            mockUi.Setup(ui => ui.PromptRequired("Enter card ID: ")).Returns("1");
            mockFacade.Setup(f => f.GetCardBalanceAsync(1)).ReturnsAsync(500.50m);

            var command = new ShowCardBalanceCommand(mockFacade.Object, mockUi.Object);

            // Act
            await command.ExecuteAsync();

            // Assert
            mockUi.Verify(ui => ui.ShowMessage("Balance for card ID 1: 500.50"), Times.Once());
            mockFacade.Verify(f => f.GetCardBalanceAsync(1), Times.Once());
        }

        [Fact]
        public async Task ExecuteAsync_InvalidCardId_ShowsError()
        {
            // Arrange
            var mockFacade = new Mock<IBankFacade>();
            var mockUi = new Mock<IConsoleUI>();

            mockUi.Setup(ui => ui.PromptRequired("Enter card ID: ")).Returns("invalid");

            var command = new ShowCardBalanceCommand(mockFacade.Object, mockUi.Object);

            // Act
            await command.ExecuteAsync();

            // Assert
            mockUi.Verify(ui => ui.ShowError("Card ID must be a number."), Times.Once());
            mockFacade.Verify(f => f.GetCardBalanceAsync(It.IsAny<int>()), Times.Never());
        }

    }
}
