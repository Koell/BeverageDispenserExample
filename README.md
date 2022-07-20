# Green Donkey Beverage Dispenser

Assume your are implementing a software for a beverage vending machine (Getränkeautomat) for the beverage company *Green Donkey*. Every product has a number (10-99), and a customer can choose which beverage she wants by typing in that number in the integrated keypad.

## Business logic

Implement a component which receives the following string messages and reacts to them:
| Message | Description |
|-|-|
| "1" .. "9" | The digit pressed by the customer. |
| "PAYMENT_SUCCEEDED" | Sent by the payment module when the payment was successful. |
| "PAYMENT_FAILED" | Sent by the payment module when the payment failed for whatever reason. |
| "PRODUCT_DISPENSED" | Sent by the product dispenser when the product was successfully given to the customer. |

That component should implement the following logic:

The vending machine starts in an *Idle* state and the message "Please select a product" is outputted:
* If a digit message is received, it is remembered and the machine waits for the next digit:
    * If more than 1 second passes, it discards the digit and goes back to the *Idle* state (writing the "Please ..." message again).
    * If a second digit message is received within a second, both numbers together form the *Product Key*. In this case the message "Please complete your payment" should be outputted and the method `IPaymentHandler.StartPayment` should be called. The dispenser now waits for a message from the payment module:
        * If the `PAYMENT_SUCCEEDED` message is received in this state, the product calls the `IProductDispenser.Dispense` method and waits for the `PRODUCT_DISPENSED` message and outputs "Your product xy is on your way":
            * If the `PRODUCT_DISPENSED` message is received, the machine goes back to the *Idle* state (writing the "Please ..." message again).
        * If the `PAYMENT_FAILED` message is received, the vending machine goes back to the *Idle* state (writing the "Please ..." message again).
        * If a digit message is received, it is ignored.

If any unexpected message (a message that is not expected or not expected at the current state as described above), the dispenser should go the the *Error* state, where it outputs the message "Out of Service" where it does not accept any further messages.

## External services

### IProductDispenser

The following interface is already given:

```csharp
/// <summary>
/// Dispenses a product to the customer by moving the carriage to the position of the 
/// product, dropping the product into the carriage, and moving the carriage to the hatch
/// (Ausgabeklappe).
/// </summary>
public interface IProductDispenser {
    /// <summary>
    /// Starts the dispense process but immediately returns and  does not wait for the 
    /// dispense process to finish. It uses some sort of callback or  messaging to notify 
    /// the caller when the process is finished.
    /// </summary>
    Task Dispense(int productId);
}
```

**Task:** The actual dispense logic is not part of this project. Provide a fake implementation with the following properties:
* `Dispense` returns `Task.CompletedTask` and then waits a random amount of time (between 500ms and 1500ms) before it somehow notifies the caller with the `PRODUCT_DISPENSED` message. Before the `PRODUCT_DISPENSED` message is sent, the message "Dispensed product xy" is outputted.


**Task:** Find a good way how the dispenser can communicate with your component.

### IPaymentHandler

The following interface is already given:

```csharp
public interface IPaymentHandler {
    /// <summary>
    /// Starts the payment process but immediately returns and  does not wait for the 
    /// the payment process to finish. It uses some sort of callback or messaging to notify the
    ///  caller when the process is finished.
    /// </summary>
    Task StartPayment(int productId);
}
```

**Task:** The actual payment logic is not part of this project. Provide a fake implementation with the following properties:
* `StartPayment` returns `Task.CompletedTask` and then waits a random amount of time (between 500ms and 1500ms) before it somehow notifies the caller with either the `PAYMENT_SUCCEEDED` or the `PAYMENT_FAILED` message (decided randomly). Before the `PAYMENT_FAILED` message is send, the message "Payment failed" is outputted by the payment handler.


**Task:** Find a good way how the payment handler should communicate with your component.

## Nonfunctional Requirements

If possible, you should consider the following nonfunctional requirements. If you are for any reason not able implement them, you may skip/ignore them (especially the Task/async part).

* Leverage the full power of object orientation and polymorphism to make the implementation of your component as clear and maintainable as possible.
* Since multiple messages can arrive at the same time the implementation of your component has to be thread safe.
* Use the Task/async pattern wherever possible and sensible.
* Use clean code/best practices where applicable (KISS, DRY, DI, IoC, ...)

## Tasks

1. Implement the logic described above in C#. If you are not familiar with C# at all, you might consider implementing the example in a language of your choice. 
1. Implement a console program that continuously asks the user to enter message strings (empty inputs are ignored) and sends the messages to your component.
1. If you can, provide unit tests for your component written in a test framework of your choice (but it should be supported by Visual Studio if you are using C#).
1. Document your solution reasonably (use common sense, what is already clear doesn't have to be documented).

## Notes

If you do not fully understand the requirements, make reasonable assumption. If anything is not clear to you, or you are unable to implement some parts, feel free to change the project to suit your skill level and available time. You can also extend/modify the example if you wish to do so. The primary goal of this exercise is to see some good code written by you.

## Example

Running the program could for example produce the following outputs:

```
=> Please select a product
Input: 1
Input: 2
=> Please complete your payment
Input: 5
Input: 2
=> Your product 12 is on your way
Input: 
=> Please select a product
Input: 1
Input: 
=> Please select a product
Input: 
```
