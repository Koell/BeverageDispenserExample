using System.Collections.Concurrent;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Timers;
using BeverageDispenser.PaymentHandler;
using BeverageDispenser.ProductDispenser;
using Timer = System.Timers.Timer;

namespace BeverageDispenser {
    public class VendingMachine {
        private enum State {
            Idle,
            WaitingForPayment,
            Dispense,
            Error
        }
        
        private static Dictionary<string, string> _validMessages = new Dictionary<string, string>() {
            { "number", @"^\d$" },
            { "payment_succeeded", @"^PAYMENT_SUCCEEDED$" },
            { "payment_failed", @"^PAYMENT_FAILED$" },
            { "product_dispensed", @"^PRODUCT_DISPENSED$" }
        };

        private readonly ConcurrentQueue<string> _messageQueue = new ConcurrentQueue<string>();
        private State _state = State.Idle;
        private string _productKey = string.Empty;
        private string _paymentInfo = string.Empty;
        private IProductDispenser _productDispenser;
        private IPaymentHandler _paymentHandler;
        private readonly Timer _timerIdleTimeout = new Timer(1000);
        
        public bool Running { get; } = true;


        public VendingMachine(IProductDispenser productDispenser, IPaymentHandler paymentHandler) {
            _productDispenser = productDispenser;
            _paymentHandler = paymentHandler;
            _timerIdleTimeout.Elapsed += new ElapsedEventHandler(OnIdleTimeoutEvent);
        }

        public void ResetMachine() {
            ChangeState(State.Idle);
            _productKey = string.Empty;
            _paymentInfo = string.Empty;
            Task.Run(ReadAsync);
        }

        private async Task ReadAsync() {
            string message = await Console.In.ReadLineAsync() ?? string.Empty;;
            if (ValidateMessage(message)) {
                SendMessage(message);
            } else {
                await Task.Run(ReadAsync);
            }
        }

        private bool ValidateMessage(string message) {
            return _validMessages.Values.Any(pattern => Regex.IsMatch(message, pattern));
        }

        private void SendMessage(string message) {
            _messageQueue.Enqueue(message);

            Task.Run(ProcessMessageQueue);
        }

        private void ProcessMessageQueue() {
            while (_messageQueue.TryDequeue(out string? message)) {
                switch (_state) {
                    case State.Idle:
                        HandleIdleMessage(message);
                        break;
                    case State.WaitingForPayment:
                        HandlePaymentMessage(message);
                        break;
                    case State.Dispense:
                    case State.Error:
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private void HandleIdleMessage(string message) {
            if (Regex.IsMatch(message, _validMessages["number"])) {
                _productKey += message;
                if (_productKey.Length == 1) {
                    _timerIdleTimeout.Start();
                } else {
                    _timerIdleTimeout.Stop();
                    ChangeState(State.WaitingForPayment);
                }
            }
            Task.Run(ReadAsync);
        }

        private void OnIdleTimeoutEvent(Object? sender, ElapsedEventArgs e) {
            if (_state == State.Idle && _productKey.Length == 1) {
                ResetMachine();
            }
        }

        private void HandlePaymentMessage(string message) {
            if (Regex.IsMatch(message, _validMessages["number"])) {
                _paymentInfo += message;
                if (_paymentInfo.Length == 2) {
                    
                    Task paymentTask = _paymentHandler.StartPayment(int.Parse(_productKey));
                    bool paymentTaskIsCanceled = paymentTask.IsCanceled;

                    if (!paymentTaskIsCanceled) {
                        ChangeState(State.Dispense);
                        
                    }
                   
                }
            }
            Task.Run(ReadAsync);
        }
 
        private void ChangeState(State newState) {
            string oldState = _state.ToString(); 
            string? message = null;
            bool valid = false;
            
            switch (newState) {
                case State.Idle:
                    message = "Please select a product";
                    valid = true;
                    break;
                case State.WaitingForPayment:
                    if (_state == State.Idle) {
                        message = "Please complete your payment";
                        valid = true;
                    }
                    break;
                case State.Dispense:
                    if (_state == State.WaitingForPayment) {
                        message = $"Your product {_productKey} is on your way";
                        valid = true;
                    }
                    break;
                case State.Error:
                    break;
            }
            if (valid) {
                _state = newState;
                if (!string.IsNullOrEmpty(message)) {
                    Console.WriteLine(message);
                }
            } else {
                Logger.Instance?.LogError($"Invalid state change {oldState} -> {newState.ToString()}. Resetting Machine");
                _state = State.Error;
                ResetMachine();
            }
        }
        private void HandleDispenseMessage(string message) { }

    }


}
