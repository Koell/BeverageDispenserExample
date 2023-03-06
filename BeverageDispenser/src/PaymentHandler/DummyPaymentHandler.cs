namespace BeverageDispenser.PaymentHandler {
    public class DummyPaymentHandler: IPaymentHandler {
        public Task StartPayment(int productId) {
            TaskCompletionSource cts = new TaskCompletionSource();
            Random rnd = new Random(DateTime.Now.Millisecond);
            
            if (rnd.Next(0, 100) > 70) {
                cts.SetCanceled();
            }
            return cts.Task;
        }
    }
}
