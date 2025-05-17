using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank_App.AppLogic
{
    public class ConsoleUI
    {
        public void ShowMenu()
        {
            Console.WriteLine("Welcome to the Bank App!");
            Console.WriteLine("1. Create User");
            Console.WriteLine("2. Create Card");
            Console.WriteLine("3. Create Transaction");
            Console.WriteLine("4. Show All Users");
            Console.WriteLine("5. Show User Cards");
            Console.WriteLine("6. Show Card Balance");
            Console.WriteLine("7. Show User Transactions");
            Console.WriteLine("0. Exit");
            Console.Write("Choose an option: ");
        }

        public string GetInput()
        {
            return Console.ReadLine() ?? "";
        }

        public string PromptRequired(string prompt)
        {
            string? input;
            do
            {
                Console.Write(prompt);
                input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input))
                    Console.WriteLine("This field is required!");
            } while (string.IsNullOrWhiteSpace(input));

            return input;
        }

        public string PromptOptional(string prompt)
        {
            Console.Write(prompt);
            return Console.ReadLine() ?? "";
        }

        public void ShowMessage(string message)
        {
            Console.WriteLine(message);
        }

        public void ShowError(string errorMessage)
        {
            Console.WriteLine($"Error: {errorMessage}");
        }
    }
}
