using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bank_App.Data;
using Bank_App.Services;
using Bank_App.AppLogic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Bank_App
{
    static class Program
    {
        static async Task Main(string[] args)
        {
            using IHost host = Host.CreateDefaultBuilder(args)
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.SetMinimumLevel(LogLevel.Warning);
                })
                .ConfigureServices((context, services) =>
                {
                    services.AddDbContext<AppDbContext>();

                    services.AddScoped<IUserService, UserService>();
                    services.AddScoped<ICreditCardService, CreditCardService>();
                    services.AddScoped<ITransactionService, TransactionService>();
                    services.AddScoped<BankFacade>(); 
                    services.AddScoped<ICreditCardFactory, CreditCardFactory>();
                    services.AddScoped<ConsoleUI>(); 
                    services.AddScoped<InputValidator>(); 
                    services.AddScoped<App>();
                })
                .Build();

            var app = host.Services.GetRequiredService<App>();
            await app.RunAsync();
        }
    }
}

//Github repo: https://github.com/HobiTi4/Bank_App