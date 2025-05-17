using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank_App.AppLogic.Command
{
    public class CreateTransactionCommand : ICommand
    {
        private readonly BankFacade _facade;
        private readonly ConsoleUI _ui;

        public CreateTransactionCommand(BankFacade facade, ConsoleUI ui)
        {
            _facade = facade;
            _ui = ui;
        }

        public async Task ExecuteAsync()
        {
            try
            {
                var senderCardIdInput = _ui.PromptRequired("Enter sender card ID: ");
                if (!int.TryParse(senderCardIdInput, out int senderCardId))
                {
                    _ui.ShowError("Sender card ID must be a number.");
                    return;
                }

                var receiverCardIdInput = _ui.PromptRequired("Enter receiver card ID: ");
                if (!int.TryParse(receiverCardIdInput, out int receiverCardId))
                {
                    _ui.ShowError("Receiver card ID must be a number.");
                    return;
                }

                var amountInput = _ui.PromptRequired("Enter transaction amount: ");
                if (!Decimal.TryParse(amountInput, out decimal amount) || amount <= 0)
                {
                    _ui.ShowError("Amount must be a positive number.");
                    return;
                }

                var transaction = await _facade.CreateTransactionAsync(senderCardId, receiverCardId, amount);
                if (senderCardId == receiverCardId)
                {
                    _ui.ShowMessage($"Funds of {amount} added to card ID {senderCardId}.");
                }
                else
                {
                    _ui.ShowMessage($"Transaction created with ID: {transaction.Id}");
                }
            }
            catch (Exception ex)
            {
                _ui.ShowError(ex.Message);
            }
        }
    }
}