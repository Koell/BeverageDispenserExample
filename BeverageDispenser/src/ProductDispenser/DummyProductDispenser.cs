using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeverageDispenser.ProductDispenser {
    public class DummyProductDispenser: IProductDispenser {
        public Task Dispense(int productId) {
            var rnd = new Random(DateTime.Now.Millisecond);
            Task.Delay(rnd.Next(500, 1500));
            return Task.CompletedTask;
        }
    }
}
