using System;
using System.Threading.Tasks;

namespace BeverageDispenser
{
    public class BeverageVendingMachine
    {
        private readonly IProductDispenser productDispenser;
        private readonly IPaymentHandler paymentHandler;
        private int productId = -1;
        private DateTime lastDigitTime = DateTime.MinValue;
        private State state = State.Idle;

        public BeverageVendingMachine(IProductDispenser productDispenser, IPaymentHandler paymentHandler)
        {
            this.productDispenser = productDispenser;
            this.paymentHandler = paymentHandler;
            Console.WriteLine("Please select a product");
        }

        public void HandleMessage(string message)
        {
            switch (state)
            {
                case State.Idle:
                    if (int.TryParse(message, out var digit))
                    {
                        HandleDigit(digit);
                    }
                    else
                    {
                        GoToErrorState();
                    }
                    break;
                case State.WaitingForPayment:
                    switch (message)
                    {
                        case "PAYMENT_SUCCEEDED":
                            productDispenser.Dispense(productId).ContinueWith(_ =>
                            {
                                Console.WriteLine($"Your product {productId} is on your way");
                                productId = -1;
                                state = State.Idle;
                                Console.WriteLine("Please select a product");
                            });
                            break;
                        case "PAYMENT_FAILED":
                            Console.WriteLine("Payment failed");
                            productId = -1;
                            state = State.Idle;
                            Console.WriteLine("Please select a product");
                            break;
                        case "PRODUCT_DISPENSED":
                            state = State.Idle;
                            break;
                        default:
                            // Ignore unexpected messages
                            break;
                    }
                    break;
                default:
                    GoToErrorState();
                    break;
            }
        }

        private void HandleDigit(int digit)
        {
            var currentTime = DateTime.Now;
            if (productId == -1)
            {
                // First digit entered
                productId = digit;
                lastDigitTime = currentTime;
                Task.Delay(1000).ContinueWith(_ =>
                {
                    if (productId != -1 && DateTime.Now - lastDigitTime > TimeSpan.FromSeconds(1))
                    {
                        productId = -1;
                        Console.WriteLine("Please select a product");
                    }
                });
            }
            else
            {
                // Second digit entered
                productId = productId * 10 + digit;
                Console.WriteLine("Please complete your payment");
                state = State.WaitingForPayment;
                paymentHandler.StartPayment(productId).ContinueWith(task =>
                {
                    if (task.Result == PaymentResult.Succeeded)
                    {
                        HandleMessage("PAYMENT_SUCCEEDED");
                    }
                    else
                    {
                        HandleMessage("PAYMENT_FAILED");
                    }
                });
            }
        }

        private void GoToErrorState()
        {
            Console.WriteLine("Out of Service");
            productId = -1;
            state = State.Idle;
        }

        private enum State
        {
            Idle,
            WaitingForPayment
        }
    }

    public interface IProductDispenser
    {
        Task Dispense(int productId);
    }

    public interface IPaymentHandler
    {
        Task<PaymentResult> StartPayment(int productId);
    }

    public enum PaymentResult
    {
        Succeeded,
        Failed
    }

    public class ProductDispenser : IProductDispenser
    {
        public async Task Dispense(int productId)
        {
            await Task.Delay(500); // simulate the time to dispense the product
            Console.WriteLine($"Product {productId} has been dispensed.");
        }
    }

    public class PaymentHandler : IPaymentHandler
    {
        private readonly Random random = new Random();

        public async Task<PaymentResult> StartPayment(int productId)
        {
            PaymentResult result = PaymentResult.Failed;
            await Task.Delay(500); // simulate delay
            if (random.Next(0, 10) > 3) {
                result = PaymentResult.Succeeded;
            }

            return result;
        }
    }
}
