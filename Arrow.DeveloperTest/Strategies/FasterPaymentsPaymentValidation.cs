using Arrow.DeveloperTest.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Arrow.DeveloperTest.Strategies
{
    public class FasterPaymentsPaymentValidation : IPaymentValidationStrategy
    {
        /// <summary>
        /// Validation for FasterPayments Payment, validation logic and structure is more readable here (subjective)
        /// </summary>
        /// <param name="account"></param>
        /// <param name="request"></param>
        /// <returns>bool</returns>
        public bool IsValid(Account account, MakePaymentRequest request)
        {
            var isValid = false; //assume not valid initially

            if (account != null)
            {
                if (account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.FasterPayments))
                {
                    if (account.Balance >= request.Amount)
                    {
                        isValid = true;
                    }
                }
            }

            return isValid;
        }
    }
}
