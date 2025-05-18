using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank_App.AppLogic.Command
{
    public class CreateUserCommand : ICommand
    {
        private readonly IBankFacade _facade;
        private readonly IConsoleUI _ui;
        private readonly InputValidator _validator;

        public CreateUserCommand(IBankFacade facade, IConsoleUI ui, InputValidator validator)
        {
            _facade = facade;
            _ui = ui;
            _validator = validator;
        }

        public async Task ExecuteAsync()
        {
            try
            {
                var name = _ui.PromptRequired("Enter user name: ");
                var surname = _ui.PromptRequired("Enter user surname: ");

                string email;
                do
                {
                    email = _ui.PromptRequired("Enter user email: ");
                    if (!_validator.IsValidEmail(email))
                        _ui.ShowError("Invalid email format. Please try again.");
                } while (!_validator.IsValidEmail(email));

                string password;
                do
                {
                    password = _ui.PromptRequired("Enter user password: ");
                    if (!_validator.IsValidPassword(password))
                        _ui.ShowError("Password must contain at least one uppercase letter, one lowercase letter, and one digit.");
                } while (!_validator.IsValidPassword(password));

                var phone = _ui.PromptOptional("Enter user phone number (optional): ");

                var user = await _facade.CreateUserAsync(name, surname, email, password, phone);
                _ui.ShowMessage($"User created with ID: {user.Id}");
            }
            catch (Exception ex)
            {
                _ui.ShowError(ex.Message);
            }
        }
    }
}
