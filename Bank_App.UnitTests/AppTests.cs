using Xunit;
using Moq;
using System.Threading.Tasks;
using Bank_App.AppLogic;
using Bank_App.Services;
using Bank_App.Models;
using System.Collections.Generic;

namespace Bank_App.AppLogic.Tests
{
    public class AppTests
    {
        [Fact]
        public async Task CreateUserFlowAsync_CreateUserSuccessfully()
        {
            // Arrange
            var mockUserService = new Mock<IUserService>();
            var mockCardService = new Mock<ICreditCardService>();
            var mockTransactionService = new Mock<ITransactionService>();

            var expectedUser = new User
            {
                Id = 1,
                Name = "Hello",
                Surname = "World",
                Email = "HelloWorld@example.com",
                Password = "Pass123"
            };

            mockUserService
                .Setup(s => s.CreateUserAsync("Hello", "World", "HelloWorld@example.com", "Pass123", "0987654321"))
                .ReturnsAsync(expectedUser);

            var inputs = new Queue<string>(new[]
            {
                "Hello",
                "World",
                "HelloWorld@example.com",
                "pass123",
                "Pass123",
                "0987654321"
            });

            var app = new InlineInputApp(
                mockUserService.Object,
                mockCardService.Object,
                mockTransactionService.Object,
                inputs
            );

            // Act
            await app.Test_CreateUserFlowAsync();

            // Assert
            mockUserService.Verify(s =>
                s.CreateUserAsync("Hello", "World", "HelloWorld@example.com", "Pass123", "0987654321"), Times.Once);
        }

        private class InlineInputApp : App
        {
            private readonly Queue<string> _inputs;

            public InlineInputApp(
                IUserService userService,
                ICreditCardService cardService,
                ITransactionService transactionService,
                Queue<string> inputs)
                : base(userService, cardService, transactionService)
            {
                _inputs = inputs;
            }

            protected override string PromptRequired(string prompt)
            {
                return _inputs.Dequeue();
            }

            protected override string PromptOptional(string prompt)
            {
                return _inputs.Count > 0 ? _inputs.Dequeue() : "";
            }

            public async Task Test_CreateUserFlowAsync()
            {
                await CreateUserFlowAsync();
            }
        }

        [Fact]
        public void IsValidPassword_ValidPassword_ReturnsTrue()
        {
            //Arrange
            var app = new App(null, null, null);
            //Act
            var result = app.IsValidPassword("validPassword123");
            //Assert
            Assert.True(result);
        }
        [Fact]
        public void IsValidPassword_ValidPassword_ReturnsFalse()
        {
            //Arrange
            var app = new App(null, null, null);
            //Act
            var result = app.IsValidPassword("notvalidpassword");
            //Assert
            Assert.False(result);
        }
        [Fact]
        public void IsValidEmail_ValidEmail_ReturnsTrue()
        {
            // Arrange
            var app = new App(null, null, null);
            // Act
            var result = app.IsValidEmail("valid@gmail.com");
            // Assert
            Assert.True(result);
        }
    }
}
