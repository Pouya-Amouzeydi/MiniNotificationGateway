using MiniNotificationGateway.Console.Application.Abstractions.Factories;
using MiniNotificationGateway.Console.Application.Abstractions.Security;
using MiniNotificationGateway.Console.Application.Factories;
using MiniNotificationGateway.Console.Domain.Events;
using MiniNotificationGateway.Console.Infrastructure.Security;

Console.WriteLine("Mini Notification Gateway");
Console.WriteLine("Stage 4 State Pattern completed.");
Console.WriteLine();

IOtpCodeGenerator otpCodeGenerator = new SixDigitOtpCodeGenerator();
IMessageFactory messageFactory = new OtpMessageFactory(otpCodeGenerator);

Console.WriteLine("Creating Message...");

var message = messageFactory.Create("09123456789");

var createdEvent = new NotificationEvent(
    eventType: NotificationEventType.MessageCreated,
    messageId: message.MessageId,
    description: "Message Created");

Console.WriteLine("Message Created");
Console.WriteLine();

Console.WriteLine("Message Information:");
Console.WriteLine($"Message Id: {message.MessageId}");
Console.WriteLine($"Recipient: {message.Recipient}");
Console.WriteLine($"Content: {message.Content}");
Console.WriteLine($"Created At: {message.CreatedAt}");
Console.WriteLine($"Current Status: {message.Status}");
Console.WriteLine($"Current State: {message.CurrentStateName}");
Console.WriteLine();

Console.WriteLine("Changing status to Sending...");
message.MarkAsSending();
Console.WriteLine($"Current Status: {message.Status}");
Console.WriteLine($"Current State: {message.CurrentStateName}");
Console.WriteLine();

Console.WriteLine("Changing status to Sent...");
message.MarkAsSent();
Console.WriteLine($"Final Status: {message.Status}");
Console.WriteLine($"Final State: {message.CurrentStateName}");
Console.WriteLine();

Console.WriteLine("Event Information:");
Console.WriteLine($"Event Id: {createdEvent.EventId}");
Console.WriteLine($"Event Type: {createdEvent.EventType}");
Console.WriteLine($"Event Description: {createdEvent.Description}");