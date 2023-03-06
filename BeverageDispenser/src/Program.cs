using BeverageDispenser.PaymentHandler;
using BeverageDispenser.ProductDispenser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeverageDispenser {
    public class Program {
        public static void Main(string[] args) {
            VendingMachine machine = new VendingMachine(new DummyProductDispenser(), new DummyPaymentHandler());
            machine.ResetMachine();
            while (machine.Running) { }

        }
    }
}
