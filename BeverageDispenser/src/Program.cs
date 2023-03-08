using System;

namespace BeverageDispenser
{
    public class Program
    {
        static void Main(string[] args)
        {
            IPaymentHandler handlerPayment = new PaymentHandler();
            IProductDispenser handlerProduct = new ProductDispenser();
            var vendingMachine = new BeverageVendingMachine(handlerProduct, handlerPayment);

            while (true)
            {
                var message = Console.ReadLine();
                vendingMachine.HandleMessage(message);
            }
        }
    }
}