using Arrow.DeveloperTest.Data;
using Arrow.DeveloperTest.Services;
using Arrow.DeveloperTest.Types;
using Moq;
using System.Security.Principal;
using Xunit;

namespace Arrow.DeveloperTest.Tests
{
    public class BacsPaymentServiceTests
    {
        [Fact]
        public void MakePayment_BacsScheme_SuccessfulPayment_UpdateAccountCalled()
        {
            // Arrange
            var mockAccountDataStore = new Mock<IAccountDataStore>();
            var paymentService = new PaymentService(mockAccountDataStore.Object);
            var origBalance = 100;
            var paymentRequest = new MakePaymentRequest
            {
                DebtorAccountNumber = "ExistingAccount",
                PaymentScheme = PaymentScheme.Bacs,
                Amount = 10
            };
            var account = new Account
            {
                AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs,
                Balance = origBalance
            };

            mockAccountDataStore.Setup(x => x.GetAccount("ExistingAccount")) // Mock the GetAccount method to return a valid account with Bacs allowed
                                .Returns(account);

            // Act
            var result = paymentService.MakePayment(paymentRequest);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(origBalance - paymentRequest.Amount, account.Balance);
            mockAccountDataStore.Verify(x => x.UpdateAccount(It.IsAny<Account>()), Times.Once); // Verify that UpdateAccount was called
        }

        [Fact]
        public void MakePayment_BacsScheme_AccountNotFound_Failure()
        {
            // Arrange
            var mockAccountDataStore = new Mock<IAccountDataStore>();
            var bacsPaymentService = new PaymentService(mockAccountDataStore.Object);
            var request = new MakePaymentRequest
            {
                DebtorAccountNumber = "NonExistingAccount",
                PaymentScheme = PaymentScheme.Bacs
            };
            mockAccountDataStore.Setup(x => x.GetAccount("ExistingAccount")) // Mock the GetAccount method to return a null account - not found - with Bacs allowed
                                .Returns((Account)null);

            // Act
            var result = bacsPaymentService.MakePayment(request);

            // Assert
            Assert.False(result.Success);            
            mockAccountDataStore.Verify(x => x.UpdateAccount(It.IsAny<Account>()), Times.Never); // Verify that UpdateAccount was NOT called
        }

        [Fact]
        public void MakePayment_BacsScheme_NotAllowed_Failure()
        {
            // Arrange
            var mockAccountDataStore = new Mock<IAccountDataStore>();
            var bacsPaymentService = new PaymentService(mockAccountDataStore.Object);
            var request = new MakePaymentRequest
            {
                DebtorAccountNumber = "ExistingAccount",
                PaymentScheme = PaymentScheme.Bacs
            };
            
            mockAccountDataStore.Setup(x => x.GetAccount("ExistingAccount")) // Mock the GetAccount method to return a valid account with Bacs not allowed
                                .Returns(new Account { AllowedPaymentSchemes = AllowedPaymentSchemes.Chaps }); //NOT Bacs

            // Act
            var result = bacsPaymentService.MakePayment(request);

            // Assert
            Assert.False(result.Success);            
            mockAccountDataStore.Verify(x => x.UpdateAccount(It.IsAny<Account>()), Times.Never); // Verify that UpdateAccount was NOT called
        }

        [Fact]
        public void MakePayment_ChapsScheme_SuccessfulPayment_UpdateAccountCalled()
        {
            // Arrange
            var mockAccountDataStore = new Mock<IAccountDataStore>();
            var paymentService = new PaymentService(mockAccountDataStore.Object);
            var origBalance = 100;
            var paymentRequest = new MakePaymentRequest
            {
                DebtorAccountNumber = "ExistingAccount",
                PaymentScheme = PaymentScheme.Chaps,
                Amount = 10
            };
            var account = new Account
            {
                AllowedPaymentSchemes = AllowedPaymentSchemes.Chaps,
                Balance = origBalance
            };

            mockAccountDataStore.Setup(x => x.GetAccount("ExistingAccount")) // Mock the GetAccount method to return a valid account with Chaps allowed
                                .Returns(account);

            // Act
            var result = paymentService.MakePayment(paymentRequest);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(origBalance - paymentRequest.Amount, account.Balance);
            mockAccountDataStore.Verify(x => x.UpdateAccount(It.IsAny<Account>()), Times.Once); // Verify that UpdateAccount was called
        }

        [Fact]
        public void MakePayment_ChapsScheme_AccountNotFound_Failure()
        {
            // Arrange
            var mockAccountDataStore = new Mock<IAccountDataStore>();
            var bacsPaymentService = new PaymentService(mockAccountDataStore.Object);
            var request = new MakePaymentRequest
            {
                DebtorAccountNumber = "NonExistingAccount",
                PaymentScheme = PaymentScheme.Chaps
            };
            mockAccountDataStore.Setup(x => x.GetAccount("ExistingAccount")) // Mock the GetAccount method to return a null account - not found - with Chaps allowed
                                .Returns((Account)null);

            // Act
            var result = bacsPaymentService.MakePayment(request);

            // Assert
            Assert.False(result.Success);
            mockAccountDataStore.Verify(x => x.UpdateAccount(It.IsAny<Account>()), Times.Never); // Verify that UpdateAccount was NOT called
        }

        [Fact]
        public void MakePayment_ChapsScheme_NotAllowed_Failure()
        {
            // Arrange
            var mockAccountDataStore = new Mock<IAccountDataStore>();
            var bacsPaymentService = new PaymentService(mockAccountDataStore.Object);
            var request = new MakePaymentRequest
            {
                DebtorAccountNumber = "ExistingAccount",
                PaymentScheme = PaymentScheme.Chaps
            };

            mockAccountDataStore.Setup(x => x.GetAccount("ExistingAccount")) // Mock the GetAccount method to return a valid account with Chaps not allowed
                                .Returns(new Account { AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs }); //not Chaps

            // Act
            var result = bacsPaymentService.MakePayment(request);

            // Assert
            Assert.False(result.Success);
            mockAccountDataStore.Verify(x => x.UpdateAccount(It.IsAny<Account>()), Times.Never); // Verify that UpdateAccount was NOT called
        }

        [Fact]
        public void MakePayment_FasterPaymentsScheme_SuccessfulPayment_UpdateAccountCalled()
        {
            // Arrange
            var mockAccountDataStore = new Mock<IAccountDataStore>();
            var paymentService = new PaymentService(mockAccountDataStore.Object);
            var origBalance = 100;
            var paymentRequest = new MakePaymentRequest
            {
                DebtorAccountNumber = "ExistingAccount",
                PaymentScheme = PaymentScheme.FasterPayments,
                Amount = 10
            };
            var account = new Account
            {
                AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments,
                Balance = origBalance
            };

            mockAccountDataStore.Setup(x => x.GetAccount("ExistingAccount")) // Mock the GetAccount method to return a valid account with FasterPayments allowed
                                .Returns(account);

            // Act
            var result = paymentService.MakePayment(paymentRequest);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(origBalance - paymentRequest.Amount, account.Balance);
            mockAccountDataStore.Verify(x => x.UpdateAccount(It.IsAny<Account>()), Times.Once); // Verify that UpdateAccount was called
        }

        [Fact]
        public void MakePayment_FasterPaymentsScheme_AccountNotFound_Failure()
        {
            // Arrange
            var mockAccountDataStore = new Mock<IAccountDataStore>();
            var bacsPaymentService = new PaymentService(mockAccountDataStore.Object);
            var request = new MakePaymentRequest
            {
                DebtorAccountNumber = "NonExistingAccount",
                PaymentScheme = PaymentScheme.FasterPayments
            };
            mockAccountDataStore.Setup(x => x.GetAccount("ExistingAccount")) // Mock the GetAccount method to return a null account - not found - with FasterPayments allowed
                                .Returns((Account)null);

            // Act
            var result = bacsPaymentService.MakePayment(request);

            // Assert
            Assert.False(result.Success);
            mockAccountDataStore.Verify(x => x.UpdateAccount(It.IsAny<Account>()), Times.Never); // Verify that UpdateAccount was NOT called
        }

        [Fact]
        public void MakePayment_FasterPaymentsScheme_NotAllowed_Failure()
        {
            // Arrange
            var mockAccountDataStore = new Mock<IAccountDataStore>();
            var bacsPaymentService = new PaymentService(mockAccountDataStore.Object);
            var request = new MakePaymentRequest
            {
                DebtorAccountNumber = "ExistingAccount",
                PaymentScheme = PaymentScheme.FasterPayments
            };

            mockAccountDataStore.Setup(x => x.GetAccount("ExistingAccount")) // Mock the GetAccount method to return a valid account with FasterPayments not allowed
                                .Returns(new Account { AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs }); //not FasterPayments

            // Act
            var result = bacsPaymentService.MakePayment(request);

            // Assert
            Assert.False(result.Success);
            mockAccountDataStore.Verify(x => x.UpdateAccount(It.IsAny<Account>()), Times.Never); // Verify that UpdateAccount was NOT called
        }


    }
}