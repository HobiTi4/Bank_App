using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank_App.AppLogic.Command
{
    public class ShowCardBalanceCommand : ICommand
    {
        private readonly BankFacade _facade;
        private readonly ConsoleUI _ui;

        public ShowCardBalanceCommand(BankFacade facade, ConsoleUI ui)
        {
            _facade = facade;
            _ui = ui;
        }

        public async Task ExecuteAsync()
        {
            try
            {
                var cardIdInput = _ui.PromptRequired("Enter card ID: ");
                if (!int.TryParse(cardIdInput, out int cardId))
                {
                    _ui.ShowError("Card ID must be a number.");
                    return;
                }

                var balance = await _facade.GetCardBalanceAsync(cardId);
                _ui.ShowMessage($"Balance for card ID {cardId}: {balance}");
            }
            catch (Exception ex)
            {
                _ui.ShowError(ex.Message);
            }
        }
    }
}