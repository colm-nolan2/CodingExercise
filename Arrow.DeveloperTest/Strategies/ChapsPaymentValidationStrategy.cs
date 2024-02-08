using Arrow.DeveloperTest.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Arrow.DeveloperTest.Strategies
{
    public class ChapsPaymentValidationStrategy : IPaymentValidationStrategy
    {
        public bool IsValid(Account account, MakePaymentRequest request)
        {
            return account != null &&
                   account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.Chaps) &&
                   account.Status == AccountStatus.Live;
        }
    }
}
