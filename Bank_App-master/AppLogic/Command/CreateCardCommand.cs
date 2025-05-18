using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank_App.AppLogic.Command
{
    public class CreateCardCommand : ICommand
    {
        private readonly IBankFacade _facade;
        private readonly IConsoleUI _ui;

        public CreateCardCommand(IBankFacade facade, ConsoleUI ui)
        {
            _facade = facade;
            _ui = ui;
        }

        public async Task ExecuteAsync()
        {
            try
            {
                var userIdInput = _ui.PromptRequired("Enter user ID: ");
                if (!int.TryParse(userIdInput, out int userId))
                {
                    _ui.ShowError("ID must be a number.");
                    return;
                }

                var card = await _facade.CreateCardAsync(userId);
                _ui.ShowMessage($"Card created with ID: {card.Id}");
            }
            catch (Exception ex)
            {
                _ui.ShowError(ex.Message);
            }
        }
    }
}
