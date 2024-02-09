using Arrow.DeveloperTest.Types;
using Microsoft.Extensions.Logging;

namespace Arrow.DeveloperTest.Data
{
    public class AccountDataStore : IAccountDataStore
    {
        private readonly ILogger<AccountDataStore> _logger;
        public AccountDataStore(ILogger<AccountDataStore> logger)
        {
            _logger = logger;
        }
        public Account GetAccount(string accountNumber)
        {
            var accountBalance = 100;

            _logger.LogInformation($"Retrieved account with Bacs Allowed Payment Schemes and a Balance of ${accountBalance}");

            // Access database to retrieve account, code removed for brevity, dummy account below for the purposes of the console app
            return new Account
            {
                AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs,
                Balance = accountBalance
            };
        }

        public void UpdateAccount(Account account)
        {
            _logger.LogInformation($"Account updated with a balance of {account.Balance}");

            // Update account in database, code removed for brevity
        }
    }
}
