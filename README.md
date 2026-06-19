# Mini Notification Gateway

Final project for the **Design Patterns in C#** course.

## Project Description

Mini Notification Gateway is a simple notification sending system that creates and sends OTP messages through multiple SMS providers.

The system starts sending the message with `ProviderA`. If `ProviderA` fails, it automatically switches to `ProviderB`.

This project focuses on correct usage of Design Patterns, SOLID principles, Dependency Injection, clean class design, and readable code.

---

## Main Scenario

The expected demo scenario is:

```text
Creating Message...
Message Created
Selecting Provider...
ProviderA Selected
Sending Message...
ProviderA Failed
Switching Provider...
ProviderB Selected
Sending Message...
Message Sent Successfully
Final Status: Sent
```

---

## Requirements Implemented

* Create OTP messages
* Generate OTP content
* Send messages through SMS providers
* Support two providers:

  * ProviderA: 70% success rate
  * ProviderB: 90% success rate
* Automatic failover from ProviderA to ProviderB
* Message status management
* Event logging
* Clean folder structure
* Dependency Injection
* SOLID principles
* No large switch statements for provider selection
* No chained if/else for provider selection
* Interface-based design

---

## Technologies

* C#
* .NET 10
* Console Application
* Microsoft.Extensions.DependencyInjection
* Visual Studio 2026

---

## Project Structure

```text
MiniNotificationGateway.Console
│
├── Application
│   ├── Abstractions
│   ├── Commands
│   ├── Decorators
│   ├── Events
│   ├── Facades
│   ├── Factories
│   ├── Providers
│   ├── Proxies
│   ├── Results
│   └── Strategies
│
├── DependencyInjection
│
├── Domain
│   ├── Common
│   ├── Events
│   ├── Messages
│   └── Providers
│
├── Infrastructure
│   ├── Logging
│   ├── Providers
│   └── Security
│
├── Presentation
│   └── ConsoleOutput
│
└── Program.cs
```

---

## Design Patterns Used

### 1. Factory Method

Used in:

```text
OtpMessageFactory
```

The system does not create OTP messages directly in `Program.cs`.
Instead, message creation is handled by `OtpMessageFactory`.

This factory creates a message with OTP content like:

```text
Your verification code is 123456
```

---

### 2. Adapter Pattern

Used in:

```text
ProviderAAdapter
ProviderBAdapter
```

`ProviderAClient` and `ProviderBClient` have different APIs.

ProviderA uses:

```text
SendSmsAsync(phoneNumber, text)
```

ProviderB uses:

```text
DeliverAsync(request)
```

The adapters convert both providers to the shared interface:

```text
INotificationProvider
```

This allows the application to work with different providers through one common contract.

---

### 3. Chain of Responsibility

Used in:

```text
NotificationProviderHandler
```

The provider failover logic is implemented as a chain:

```text
ProviderAHandler → ProviderBHandler
```

If ProviderA fails, the request is passed to ProviderB automatically.

This removes the need for large switch statements or chained if/else blocks for provider selection.

---

### 4. State Pattern

Used in:

```text
CreatedMessageState
SendingMessageState
SentMessageState
FailedMessageState
```

The message has different states:

```text
Created
Sending
Sent
Failed
```

Each state controls which transition is allowed.

For example, a message cannot go directly from `Created` to `Sent`; it must first move to `Sending`.

---

### 5. Observer Pattern

Used in:

```text
NotificationEventPublisher
InMemoryEventObserver
ConsoleEventObserver
```

System events are published and stored as notification events.

Implemented events:

```text
MessageCreated
ProviderSelected
MessageSent
MessageFailed
ProviderChanged
```

This allows the system to log important events without tightly coupling the core sending logic to logging details.

---

### 6. Command Pattern

Used in:

```text
SendOtpCommand
CommandInvoker
```

The OTP sending operation is encapsulated inside `SendOtpCommand`.

The command is responsible for:

* Changing message status to `Sending`
* Executing the sending strategy
* Updating final status to `Sent` or `Failed`
* Returning the send result

---

### 7. Strategy Pattern

Used in:

```text
FailoverSendingStrategy
```

The sending policy is separated from the command.

Currently, the project uses a failover strategy:

```text
Start from ProviderA.
If ProviderA fails, continue with ProviderB.
```

Other strategies can be added later without changing `SendOtpCommand`.

---

### 8. Facade Pattern

Used in:

```text
NotificationGatewayFacade
```

The facade provides a simple entry point:

```text
SendOtpAsync(recipient)
```

It hides the complexity of:

* Creating the message
* Publishing events
* Creating the command
* Executing the command
* Returning the response

---

### 9. Decorator Pattern

Used in:

```text
LoggingNotificationGatewayFacadeDecorator
```

The decorator adds logging behavior around the gateway without modifying the original `NotificationGatewayFacade`.

---

### 10. Proxy Pattern

Used in:

```text
RateLimitedNotificationGatewayFacadeProxy
```

The proxy controls access to the gateway by limiting OTP requests per recipient.

It checks request count before forwarding the request to the real gateway.

---

## SOLID Principles

### Single Responsibility Principle

Each class has one clear responsibility.

Examples:

* `OtpMessageFactory`: creates OTP messages
* `SendOtpCommand`: executes OTP sending
* `NotificationGatewayFacade`: coordinates the main scenario
* `RateLimitedNotificationGatewayFacadeProxy`: controls request limits
* `DemoOutputWriter`: writes demo output

---

### Open/Closed Principle

The project can be extended without modifying existing core classes.

Examples:

* New providers can be added by implementing `INotificationProvider`
* New observers can be added by implementing `IEventObserver`
* New sending strategies can be added by implementing `ISendingStrategy`

---

### Liskov Substitution Principle

Implementations can be replaced through their interfaces.

Examples:

* `ProviderAAdapter` and `ProviderBAdapter` can both be used as `INotificationProvider`
* `NotificationGatewayFacade`, its decorator, and its proxy can all be used as `INotificationGatewayFacade`

---

### Interface Segregation Principle

Interfaces are small and focused.

Examples:

```text
IMessageFactory
IOtpCodeGenerator
INotificationProvider
INotificationEventPublisher
IEventObserver
ISendingStrategy
ICommand
INotificationGatewayFacade
```

Each interface describes only the behavior needed by its clients.

---

### Dependency Inversion Principle

High-level modules depend on abstractions, not concrete classes.

Dependency Injection is configured in:

```text
ServiceCollectionExtensions
```

`Program.cs` resolves the main gateway through:

```text
INotificationGatewayFacade
```

---

## Dependency Injection

Dependency Injection is configured in:

```text
DependencyInjection/ServiceCollectionExtensions.cs
```

`Program.cs` does not manually create providers, factories, strategies, commands, or facades.

It only builds the service provider and resolves:

```text
INotificationGatewayFacade
InMemoryEventObserver
DemoOutputWriter
```

---

## Demo Output

Sample output:

```text
Mini Notification Gateway
Final Demo Scenario

Creating Message...
Message Created
Selecting Provider...
ProviderA Selected
Sending Message...
ProviderA Failed
Switching Provider...
ProviderB Selected
Sending Message...
Message Sent Successfully
Final Status: Sent

Event Log:
- MessageCreated | Message Created
- ProviderSelected | ProviderA Selected
- MessageFailed | ProviderA Failed
- ProviderChanged | Switching Provider...
- ProviderSelected | ProviderB Selected
- MessageSent | Message Sent Successfully
```

---

## How to Run

1. Open the solution in Visual Studio 2026.
2. Set `MiniNotificationGateway.Console` as the startup project.
3. Run the project.
4. The console displays the final demo scenario.

---

## Notes

This project intentionally uses simulated providers.

No real SMS provider API is used.

Provider results are controlled in the demo to guarantee this scenario:

```text
ProviderA fails.
ProviderB succeeds.
Final Status is Sent.
```

---

## Author

Pouya Amouzeydi

---

## Course

Design Patterns in C#
