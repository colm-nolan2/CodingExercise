using Arrow.DeveloperTest.Types;

namespace Arrow.DeveloperTest.Strategies
{
    public class BacsPaymentValidationStrategy : IPaymentValidationStrategy
    {
        /// <summary>
        /// Validation for IsValid Payment, validation logic and structure is more readable here (subjective)
        /// </summary>
        /// <param name="account"></param>
        /// <param name="request"></param>
        /// <returns>bool</returns>
        public bool IsValid(Account account, MakePaymentRequest request)
        {
            var isValid = false; //assume not valid initially

            if (account != null)
            {
                if (account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.Bacs))
                {
                    isValid = true;
                }
            }

            return isValid;
        }       
    }
}
