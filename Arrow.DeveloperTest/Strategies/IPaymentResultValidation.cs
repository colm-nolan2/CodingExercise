using Arrow.DeveloperTest.Types;

namespace Arrow.DeveloperTest.Strategies
{
    public interface IPaymentValidationStrategy
    {
        bool IsValid(Account account, MakePaymentRequest request);
    }
}
