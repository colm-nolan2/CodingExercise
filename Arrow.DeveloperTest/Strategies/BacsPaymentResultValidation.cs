using Arrow.DeveloperTest.Types;

namespace Arrow.DeveloperTest.Strategies
{
    internal class BacsPaymentValidationStrategy : IPaymentValidationStrategy
    {
        public bool IsValid(Account account, MakePaymentRequest request)
        {
            return account != null && 
                   account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.Bacs);
        }       
    }
}
