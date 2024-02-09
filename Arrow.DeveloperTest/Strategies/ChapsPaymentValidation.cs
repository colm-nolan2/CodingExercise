using Arrow.DeveloperTest.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Arrow.DeveloperTest.Strategies
{
    public class ChapsPaymentValidation : IPaymentValidationStrategy
    {
        /// <summary>
        /// Validation for Chaps Payment, validation logic and structure is more readable here (subjective)
        /// </summary>
        /// <param name="account"></param>
        /// <param name="request"></param>
        /// <returns>bool</returns>
        public bool IsValid(Account account, MakePaymentRequest request)
        {            
            var isValid = false; //assume not valid initially

            if (account != null)
            {
                if (account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.Chaps))
                {
                    if (account.Status == AccountStatus.Live)
                    {
                        isValid = true;
                    }
                }
            }

            return isValid;
        }
    }
}
