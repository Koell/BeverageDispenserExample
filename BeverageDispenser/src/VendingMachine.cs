
using System.Collections.Concurrent;
using System.ComponentModel.Design;
using BeverageDispenser.src.PaymentHandler;
using BeverageDispenser.src.ProductDispenser;

namespace BeverageDispenser.src
{
    public class VendingMachine
    {
        public enum State
        {
            Idle,
            WaitingForPayment,
            Dispense,
            Error
        }

        public enum Message
        {
            PAYMENT_SUCCEEDED,
            PAYMENT_FAILED,
            PRODUCT_DISPENSED
        }

        private ConcurrentQueue<string> _messageQueue = new ConcurrentQueue<string>();
        private State _state = State.Idle;
        private int? _productKey = null;
        private IProductDispenser _productDispenser;
        private IPaymentHandler _paymentHandler;


        public VendingMachine(IProductDispenser productDispenser, IPaymentHandler paymentHandler)
        {
            _productDispenser = productDispenser;
            _paymentHandler = paymentHandler;
        }

        public void ResetMachine()
        {
            _state = State.Idle;
            _productKey = null;

            Console.WriteLine("Please select a product");
            Task.Run(() => ReadAsync());
        }

        private async Task ReadAsync()
        {
            string msg = await Console.In.ReadLineAsync() ?? string.Empty;

            SendMessage(msg);
        }

        public void SendMessage(string message)
        {
            _messageQueue.Enqueue(message);

            Task.Run(() => ProcessMessageQueue());
        }

        private void ProcessMessageQueue()
        {
            while (_messageQueue.TryDequeue(out string? message))
            {
                switch (_state)
                {
                    case State.Idle:
                        if (message.Length == 1)
                        {
                            handleIdleMessage(message);
                        }
                        else
                        {
                            ResetMachine();
                        }
                        break;

                    case State.WaitingForPayment:
                    case State.Dispense:
                    case State.Error:
                        break;

                }
            }
        }

        private void handleIdleMessage(string message)
        {
            if (int.TryParse(message, out int number))
            {
                if (_productKey == null)
                {
                    _productKey = number;
                }
                else
                {
                    _productKey = _productKey * 10 + number;
                    //TODO
                }

                if (number == 0)
                {

                }
            }
        }

        private void handlePaymentMessage(string message) { }
        private void handleDispenseMessage(string message) { }
    }


}
