using Arrow.DeveloperTest.Types;

namespace Arrow.DeveloperTest.Strategies
{
    internal interface IPaymentValidationStrategy
    {
        bool IsValid(Account account, MakePaymentRequest request);
    }
}
