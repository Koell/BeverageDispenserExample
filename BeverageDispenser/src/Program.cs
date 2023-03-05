using BeverageDispenser.PaymentHandler;
using BeverageDispenser.ProductDispenser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeverageDispenser.src
{
    internal class Program
    {
        static void Main(string[] args)
        {
            VendingMachine machine = new VendingMachine(new DummyProductDispenser, new DummyPaymentHandler);
            machine.ResetMachine();

        }
    }
}
