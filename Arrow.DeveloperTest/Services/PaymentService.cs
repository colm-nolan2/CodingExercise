using Arrow.DeveloperTest.Data;
using Arrow.DeveloperTest.Strategies;
using Arrow.DeveloperTest.Types;
using System.Collections.Generic;

namespace Arrow.DeveloperTest.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IAccountDataStore _accountDataStore;
        private readonly Dictionary<PaymentScheme, IPaymentValidationStrategy> _paymentValidationStrategies;

        public PaymentService(IAccountDataStore accountDataStore,
                                Dictionary<PaymentScheme, IPaymentValidationStrategy> paymentValidationStrategies)
        {
            _accountDataStore = accountDataStore;
            _paymentValidationStrategies = paymentValidationStrategies;
        }

        /// <summary>
        /// Payment method
        /// </summary>
        /// <param name="request"></param>
        /// <returns>MakePaymentResult</returns>
        public MakePaymentResult MakePayment(MakePaymentRequest request)
        {
            var account = _accountDataStore.GetAccount(request.DebtorAccountNumber);

            var result = new MakePaymentResult 
            { 
                Success = IsValidPayment(account, request) 
            };

            if (result.Success)
            {
                account.Balance -= request.Amount;

                _accountDataStore.UpdateAccount(account);
            }

            return result;
        }
        
        /// <summary>
        /// Checks whether a payment request and account is valid
        /// </summary>
        /// <param name="account"></param>
        /// <param name="request"></param>
        /// <returns>bool</returns>
        private bool IsValidPayment(Account account, MakePaymentRequest request)
        {
            var isValid = false;

            if (_paymentValidationStrategies.TryGetValue(request.PaymentScheme, out var strategy))
            {
                isValid = strategy.IsValid(account, request);
            }
            
            return isValid; // Unknown payment scheme
        }
    }
}
