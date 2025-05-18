using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank_App.AppLogic.Command
{
    public class ShowUserTransactionsCommand : ICommand
    {
        private readonly IBankFacade _facade;
        private readonly IConsoleUI _ui;

        public ShowUserTransactionsCommand(IBankFacade facade, IConsoleUI ui)
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

                var transactions = await _facade.GetUserTransactionsAsync(userId);
                if (transactions.Count == 0)
                {
                    _ui.ShowMessage($"No transactions found for user with ID {userId}.");
                    return;
                }

                _ui.ShowMessage($"Transactions for user with ID {userId}:");
                foreach (var transaction in transactions)
                {
                    _ui.ShowMessage($"Transaction ID: {transaction.Id}, From Card: {transaction.SenderId}, To Card: {transaction.ReceiverId}, Amount: {transaction.Amount}, Date: {transaction.Date}");
                }
            }
            catch (Exception ex)
            {
                _ui.ShowError(ex.Message);
            }
        }
    }
}

