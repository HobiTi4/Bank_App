using Bank_App.AppLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank_App.UnitTests
{
    public class InputValidatorTests
    {
        private readonly InputValidator _validator;

        public InputValidatorTests()
        {
            _validator = new InputValidator();
        }

        [Fact]
        public void IsValidEmail_ValidEmail_ReturnsTrue()
        {
            // Arrange
            string email = "user@example.com";

            // Act
            bool result = _validator.IsValidEmail(email);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsValidEmail_InvalidEmail_ReturnsFalse()
        {
            // Arrange
            string email = "userexample.com";

            // Act
            bool result = _validator.IsValidEmail(email);

            // Assert
            Assert.False(result);
        }
        [Fact]
        public void IsValidPassword_ValidPassword_ReturnsTrue()
        {
            // Arrange
            string password = "Password123";

            // Act
            bool result = _validator.IsValidPassword(password);

            // Assert
            Assert.True(result);
        }
        [Fact]
        public void IsValidPassword_InvalidPassword_ReturnsFalse()
        {
            // Arrange
            string password = "password";

            // Act
            bool result = _validator.IsValidPassword(password);

            // Assert
            Assert.False(result);
        }

    }
}
