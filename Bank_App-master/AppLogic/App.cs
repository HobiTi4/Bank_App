using Bank_App.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bank_App.Models;

namespace Bank_App.AppLogic
{
    public class App
    {
        private readonly IUserService _userService;
        private readonly ICreditCardService _cardService;
        private readonly ITransactionService _transactionService;

        public App(IUserService userService, ICreditCardService cardService, ITransactionService transactionService)
        {
            _userService = userService;
            _cardService = cardService;
            _transactionService = transactionService;
        }

        public async Task RunAsync()
        {
            while (true)
            {
                Console.WriteLine("Welcome to the Bank App!");
                Console.WriteLine("1. Create User");
                Console.WriteLine("2. Create Card");
                Console.WriteLine("3. Create Transaction");
                Console.WriteLine("4. Print All Users");
                Console.Write("Choose option: ");
                var input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        await CreateUserFlowAsync();
                        break;
                    case "2":
                        await CreateCardFlowAsync();
                        break;
                    case "3":
                        await CreateTransactionFlowAsync();
                        break;
                    case "4":
                        await PrintAllUsersFlowAsync();
                        break;
                    case "0":
                        Console.WriteLine("Thank you for using our service! Goodbye");
                        return;
                    default:
                        Console.WriteLine("Wrong choice!");
                        break;
                }
            }
        }

        protected virtual string PromptRequired(string prompt)
        {
            string? input;
            do
            {
                Console.Write(prompt);
                input = Console.ReadLine();
            } while (string.IsNullOrWhiteSpace(input));

            return input;
        }
        protected virtual string PromptOptional(string prompt)
        {
            Console.Write(prompt);
            return Console.ReadLine() ?? "";
        }

        protected async Task CreateUserFlowAsync()
        {
            var name = PromptRequired("Enter user name: ");
            var surname = PromptRequired("Enter user surname: ");
            string email;
            do
            {
                email = PromptRequired("Enter user email: ");
                if (!IsValidEmail(email))
                {
                    Console.WriteLine("Invalid email format. Please try again.");
                }
            } while (!IsValidEmail(email));

            string password;
            do
            {
                password = PromptRequired("Enter user password: ");
                if (!IsValidPassword(password))
                {
                    Console.WriteLine("Password must contain at least one uppercase letter, one lowercase letter, and one digit");
                }
            } while (!IsValidPassword(password));

            var phone = PromptOptional("Enter user phone number (optional): ");

            var user = await _userService.CreateUserAsync(name, surname, email, password, phone);
            Console.WriteLine($"User created with ID: {user.Id}");
        }

        private async Task CreateCardFlowAsync()
        {
            var userId = int.Parse(PromptRequired("Enter user ID: "));
            var card = await _cardService.CreateCardAsync(userId);
            Console.WriteLine($"Card created with ID: {card.Id}");
        }

        private async Task CreateTransactionFlowAsync()
        {
            var senderId = int.Parse(PromptRequired("Enter sender ID: "));
            var receiverId = int.Parse(PromptRequired("Enter receiver ID: "));
            var amount = decimal.Parse(PromptRequired("Enter transaction amount: "));

            var transaction = await _transactionService.CreateTransactionAsync(senderId, receiverId, amount);
            Console.WriteLine($"Transaction created with ID: {transaction.Id}");
        }

        private async Task PrintAllUsersFlowAsync()
        {
            var users = await _userService.GetAllUsersAsync();
            foreach (var user in users)
            {
                Console.WriteLine($"ID: {user.Id}, Name: {user.Name}, Surname: {user.Surname}, Email: {user.Email}");
            }
        }
        public bool IsValidPassword(string password)
        {
            return password.Any(char.IsUpper) &&
                   password.Any(char.IsLower) &&
                   password.Any(char.IsDigit);
        }
        public bool IsValidEmail(string email)
        {
            return email.Contains("@") && email.Contains(".");
        }
    }
}
