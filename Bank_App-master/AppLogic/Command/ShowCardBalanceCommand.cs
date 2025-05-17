using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank_App.AppLogic.Command
{
    public class ShowCardBalanceCommand : ICommand
    {
        private readonly IBankFacade _facade;
        private readonly IConsoleUI _ui;

        public ShowCardBalanceCommand(IBankFacade facade, IConsoleUI ui)
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
                string balanceFormatted = balance.ToString("0.00", CultureInfo.InvariantCulture);
                _ui.ShowMessage($"Balance for card ID {cardId}: {balanceFormatted}");
            }
            catch (Exception ex)
            {
                _ui.ShowError(ex.Message);
            }
        }
    }
}