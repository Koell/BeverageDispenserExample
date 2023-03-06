using BeverageDispenser.PaymentHandler;
using BeverageDispenser.ProductDispenser;
using NUnit.Framework;

namespace BeverageDispenser.Tests
{
    [TestFixture]
    public class VendingMachineTests
    {
        [Test]
        public void TestDispenseProduct()
        {
            // Arrange
            var productDispenser = new MockProductDispenser();
            var paymentHandler = new MockPaymentHandler();
            var vendingMachine = new VendingMachine(productDispenser, paymentHandler);

            // Act
            vendingMachine.ResetMachine();
            vendingMachine.SendMessage("1");
            vendingMachine.SendMessage("2");
            vendingMachine.SendMessage("3");
            vendingMachine.SendMessage("4");
            vendingMachine.SendMessage("5");
            vendingMachine.SendMessage("6");
            vendingMachine.SendMessage("7");
            vendingMachine.SendMessage("8");
            vendingMachine.SendMessage("9");
            vendingMachine.SendMessage("0");
            vendingMachine.SendMessage("PAYMENT_SUCCEEDED");

            // Assert
            Assert.AreEqual(VendingMachine.State.Dispense, vendingMachine.Running);
            Assert.AreEqual("PRODUCT_DISPENSED", productDispenser.LastDispensedProduct);
        }

        private class MockProductDispenser : IProductDispenser
        {
            public string LastDispensedProduct { get; private set; }

            public void DispenseProduct(string productKey)
            {
                LastDispensedProduct = productKey;
            }
        }

        private class MockPaymentHandler : IPaymentHandler
        {
            public Task StartPayment(int productPrice)
            {
                return Task.FromResult(true);
            }
        }
    }
}
