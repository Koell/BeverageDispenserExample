using System.Threading;
using System.Threading.Tasks;
using System.Timers;
/*
namespace BeverageDispenserTests
{
    [TestClass]
    public class BeverageVendingMachineTests
    {
        private BeverageVendingMachine _sut;
        private Mock<IProductDispenser> _productDispenserMock;
        private Mock<IPaymentHandler> _paymentHandlerMock;

        [TestInitialize]
        public void TestInitialize()
        {
            _productDispenserMock = new Mock<IProductDispenser>();
            _paymentHandlerMock = new Mock<IPaymentHandler>();
            _sut = new BeverageVendingMachine(_productDispenserMock.Object, _paymentHandlerMock.Object);
        }

        [TestMethod]
        public async Task StartAsync_ShouldAskForProductSelection()
        {
            // Arrange
            var cancellationTokenSource = new CancellationTokenSource(500);
            var cancellationToken = cancellationTokenSource.Token;

            // Act
            await _sut.StartAsync(cancellationToken);

            // Assert
            Assert.AreEqual("Please select a product", _sut.OutputMessageEventArgs.Message);
        }

        [TestMethod]
        public async Task StartAsync_ShouldHandleInvalidInput()
        {
            // Arrange
            var cancellationTokenSource = new CancellationTokenSource(500);
            var cancellationToken = cancellationTokenSource.Token;

            // Act
            await _sut.StartAsync(cancellationToken);
            _sut.HandleDigit(42); // Invalid input

            // Assert
            Assert.AreEqual("Out of Service", _sut.OutputMessageEventArgs.Message);
        }

        [TestMethod]
        public async Task StartAsync_ShouldHandleProductSelection()
        {
            // Arrange
            var cancellationTokenSource = new CancellationTokenSource(500);
            var cancellationToken = cancellationTokenSource.Token;
            _productDispenserMock.Setup(p => p.Dispense(1)).Returns(Task.CompletedTask);
            _paymentHandlerMock.Setup(p => p.StartPayment(1)).Returns(Task.CompletedTask);

            // Act
            await _sut.StartAsync(cancellationToken);
            _sut.HandleDigit(1);

            // Assert
            Assert.AreEqual("Please complete your payment", _sut.OutputMessageEventArgs.Message);
            _paymentHandlerMock.Verify(p => p.StartPayment(1), Times.Once);
        }

        [TestMethod]
        public async Task StartAsync_ShouldHandleDelayedProductSelection()
        {
            // Arrange
            var cancellationTokenSource = new CancellationTokenSource(500);
            var cancellationToken = cancellationTokenSource.Token;
            _productDispenserMock.Setup(p => p.Dispense(1)).Returns(Task.CompletedTask);
            _paymentHandlerMock.Setup(p => p.StartPayment(1)).Returns(Task.CompletedTask);

            // Act
            await _sut.StartAsync(cancellationToken);
            _sut.HandleDigit(1);
            await Task.Delay(2000);
            _sut.ReceiveMessage("PAYMENT_SUCCEEDED");

            // Assert
            Assert.AreEqual("Your product 1 is on your way", _sut.OutputMessageEventArgs.Message);
        }
    }
}
*/
