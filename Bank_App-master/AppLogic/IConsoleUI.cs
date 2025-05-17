using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank_App.AppLogic
{
    public interface IConsoleUI
    {
        public void ShowMenu();
        public string GetInput();
        public void ShowMessage(string message);
        public void ShowError(string error);
        public string PromptRequired(string prompt);
        public string PromptOptional(string prompt);
    }
}
