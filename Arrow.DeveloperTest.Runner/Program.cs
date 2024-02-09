using Arrow.DeveloperTest.Data;
using Arrow.DeveloperTest.Services;
using Arrow.DeveloperTest.Strategies;
using Arrow.DeveloperTest.Types;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;

namespace Arrow.DeveloperTest.Runner
{
    class Program
    {
        static void Main(string[] args)
        {            
            var serviceCollection = new ServiceCollection();
            var startup = new Startup();
            startup.ConfigureServices(serviceCollection);
            var serviceProvider = serviceCollection.BuildServiceProvider();

            var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
            logger.LogInformation("Application starting...");

            try
            {
                //get service 
                var paymentService = serviceProvider.GetRequiredService<IPaymentService>();

                //make successful bacs payment
                var paymentRequest = new MakePaymentRequest
                {
                    DebtorAccountNumber = "ExistingAccount",
                    PaymentScheme = PaymentScheme.Bacs,
                    Amount = 10
                };
                MakePayment(logger, paymentService, paymentRequest);

                //Make Unsuccessful payment
                paymentRequest = new MakePaymentRequest
                {
                    DebtorAccountNumber = "AnotherExistingAccount",
                    PaymentScheme = PaymentScheme.None,
                    Amount = 101
                };

                MakePayment(logger, paymentService, paymentRequest);

                Console.WriteLine("Press enter to exit ...");
                Console.ReadLine();
                
            }
            catch (Exception ex)
            {
                logger.LogError($"An error occurred in the application with error message: {ex.Message}");
                throw;
            }
        }

        private static void MakePayment(
            ILogger<Program> logger, 
            IPaymentService paymentService, 
            MakePaymentRequest paymentRequest)
        {
            logger.LogInformation($"Attempting to make ${paymentRequest.Amount} payement payment to {paymentRequest.DebtorAccountNumber} for {paymentRequest.PaymentScheme} payment scheme.");
            var result = paymentService.MakePayment(paymentRequest);
            logger.LogInformation($"Payment {(result.Success ? "Succeeded" : "Failed")}\r\n");
        }
    }
}
