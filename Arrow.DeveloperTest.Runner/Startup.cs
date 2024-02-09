using Arrow.DeveloperTest.Data;
using Arrow.DeveloperTest.Services;
using Arrow.DeveloperTest.Strategies;
using Arrow.DeveloperTest.Types;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace Arrow.DeveloperTest.Runner
{
    public class Startup
    {
        IConfigurationRoot Configuration { get; }

        public Startup()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json");

            Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging(builder =>
            {
                builder.AddConsole();
            });
            services.AddSingleton<IConfigurationRoot>(Configuration);

            services.AddLogging(builder =>
             {
                 builder.AddConsole(); // You can customize this to use other logging providers
             })
                .AddScoped<IAccountDataStore, AccountDataStore>()
                .AddScoped<IPaymentService>(provider =>
                {
                    var accountDataStore = provider.GetRequiredService<IAccountDataStore>();

                    var bacsStrategy = new BacsPaymentValidationStrategy();
                    var fasterPaymentsStrategy = new FasterPaymentsPaymentValidation();
                    var chapsStrategy = new ChapsPaymentValidation();

                    var paymentValidationStrategies = new Dictionary<PaymentScheme, IPaymentValidationStrategy>
                                {
                                { PaymentScheme.Bacs, bacsStrategy },
                                { PaymentScheme.FasterPayments, fasterPaymentsStrategy },
                                { PaymentScheme.Chaps, chapsStrategy },
                                };

                    return new PaymentService(accountDataStore, paymentValidationStrategies);
                })
                .BuildServiceProvider();
        }
    }
}


