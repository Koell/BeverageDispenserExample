using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeverageDispenser.ProductDispenser {
    public interface IProductDispenser {
        /// <summary>
        /// Starts the dispense process but immediately returns and  does not wait for the 
        /// dispense process to finish. It uses some sort of callback or  messaging to notify 
        /// the caller when the process is finished.
        /// </summary>
        Task Dispense(int productId);
    }
}
