using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank_App.AppLogic.Command
{
    public class ShowUserCardsCommand : ICommand
    {
        private readonly BankFacade _facade;
        private readonly ConsoleUI _ui;

        public ShowUserCardsCommand(BankFacade facade, ConsoleUI ui)
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

                var cards = await _facade.GetCardsByUserIdAsync(userId);
                if (cards.Count == 0)
                {
                    _ui.ShowMessage($"No cards found for user with ID {userId}.");
                    return;
                }

                _ui.ShowMessage($"Cards for user with ID {userId}:");
                foreach (var card in cards)
                {
                    _ui.ShowMessage($"Card ID: {card.Id}, Card Number: {card.CardNumber}, Balance: {card.Balance}");
                }
            }
            catch (Exception ex)
            {
                _ui.ShowError(ex.Message);
            }
        }
    }
}
