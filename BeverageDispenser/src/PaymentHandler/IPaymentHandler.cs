using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeverageDispenser.PaymentHandler {
    public interface IPaymentHandler {
        /// <summary>
        /// Starts the payment process but immediately returns and  does not wait for the 
        /// the payment process to finish. It uses some sort of callback or messaging to notify the
        ///  caller when the process is finished.
        /// </summary>
        Task StartPayment(int productId);
    }
}
