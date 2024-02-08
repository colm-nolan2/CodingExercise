using Arrow.DeveloperTest.Data;
using Arrow.DeveloperTest.Strategies;
using Arrow.DeveloperTest.Types;

namespace Arrow.DeveloperTest.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IAccountDataStore _accountDataStore;

        public PaymentService(IAccountDataStore accountDataStore)
        {
            _accountDataStore = accountDataStore;
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
            IPaymentValidationStrategy strategy;

            switch (request.PaymentScheme)
            {
                case PaymentScheme.Bacs:
                    strategy = new BacsPaymentValidationStrategy();
                    break;

                case PaymentScheme.FasterPayments:
                    strategy = new FasterPaymentsPaymentValidationStrategy();
                    break;

                case PaymentScheme.Chaps:
                    strategy = new ChapsPaymentValidationStrategy();
                    break;

                default:
                    return false;
            }

            return strategy.IsValid(account, request);
        }
    }
}
