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

       private bool IsValidPayment(Account account, MakePaymentRequest request)
    {
        if (_paymentValidationStrategies.TryGetValue(request.PaymentScheme, out var strategy))
        {
            return strategy.IsValid(account, request);
        }

        // Unknown payment scheme
        return false;
    }
    }
}
