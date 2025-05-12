using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bank_App.Data;
using Bank_App.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Bank_App.AppLogic;

namespace Bank_App
{
    static class Program
    {
        static async Task Main(string[] args)
        {
            using IHost host = Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {
                    services.AddDbContext<AppDbContext>();

                    services.AddScoped<IUserService, UserService>();
                    services.AddScoped<ICreditCardService, CreditCardService>();
                    services.AddScoped<ITransactionService, TransactionService>();
                    services.AddScoped<App>();

                })
                .Build();

            var userService = host.Services.GetRequiredService<IUserService>();
            var cardService = host.Services.GetRequiredService<ICreditCardService>();
            var transactionService = host.Services.GetRequiredService<ITransactionService>();

            var app = host.Services.GetRequiredService<App>();
            await app.RunAsync();
        }
    }
}