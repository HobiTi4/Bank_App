using Bank_App.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bank_App.Models;
using Bank_App.AppLogic.Command;

namespace Bank_App.AppLogic
{
    public class App
    {
        private readonly Dictionary<string, ICommand> _commands;
        private readonly ConsoleUI _ui;

        public App(BankFacade facade, ConsoleUI ui, InputValidator validator)
        {
            _ui = ui;
            _commands = new Dictionary<string, ICommand>
            {
                { "1", new CreateUserCommand(facade, ui, validator) },
                { "2", new CreateCardCommand(facade, ui) },
                { "3", new CreateTransactionCommand(facade, ui) },
                { "4", new PrintAllUsersCommand(facade, ui) },
                { "5", new ShowUserCardsCommand(facade, ui) },
                { "6", new ShowCardBalanceCommand(facade, ui) },
                { "7", new ShowUserTransactionsCommand(facade, ui) }
            };
        }

        public async Task RunAsync()
        {
            while (true)
            {
                _ui.ShowMenu();
                var input = _ui.GetInput();

                if (input == "0")
                {
                    _ui.ShowMessage("Thank you for using our service! Goodbye!");
                    return;
                }

                if (_commands.TryGetValue(input, out var command))
                    await command.ExecuteAsync();
                else
                    _ui.ShowError("Invalid choice!");
            }
        }
    }
}

