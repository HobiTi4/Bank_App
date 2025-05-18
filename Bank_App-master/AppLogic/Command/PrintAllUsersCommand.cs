using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank_App.AppLogic.Command
{
    public class PrintAllUsersCommand : ICommand
    {
        private readonly IBankFacade _facade;
        private readonly IConsoleUI _ui;

        public PrintAllUsersCommand(IBankFacade facade, IConsoleUI ui)
        {
            _facade = facade;
            _ui = ui;
        }

        public async Task ExecuteAsync()
        {
            try
            {
                var users = await _facade.GetAllUsersAsync();
                if (users.Count == 0)
                {
                    _ui.ShowMessage("No users found.");
                    return;
                }

                _ui.ShowMessage("List of all users:");
                foreach (var user in users)
                {
                    _ui.ShowMessage($"ID: {user.Id}, Name: {user.Name}, Surname: {user.Surname}, Email: {user.Email}");
                }
            }
            catch (Exception ex)
            {
                _ui.ShowError(ex.Message);
            }
        }
    }
}
