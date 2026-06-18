using MiniNotificationGateway.Console.Domain.Common;
using MiniNotificationGateway.Console.Domain.Events;
using MiniNotificationGateway.Console.Domain.Messages;
using MiniNotificationGateway.Console.Domain.Providers;

Console.WriteLine("Mini Notification Gateway");
Console.WriteLine("Stage 2 domain models completed.");
Console.WriteLine();

var message = new Message(
    recipient: "09123456789",
    content: "Your verification code is 123456");

var createdEvent = new NotificationEvent(
    eventType: NotificationEventType.MessageCreated,
    messageId: message.MessageId,
    description: "Message Created");

Console.WriteLine("Message Information:");
Console.WriteLine($"Message Id: {message.MessageId}");
Console.WriteLine($"Recipient: {message.Recipient}");
Console.WriteLine($"Content: {message.Content}");
Console.WriteLine($"Created At: {message.CreatedAt}");
Console.WriteLine($"Current Status: {message.Status}");
Console.WriteLine();

Console.WriteLine("Event Information:");
Console.WriteLine($"Event Id: {createdEvent.EventId}");
Console.WriteLine($"Event Type: {createdEvent.EventType}");
Console.WriteLine($"Event Description: {createdEvent.Description}");
Console.WriteLine();

message.MarkAsSending();
Console.WriteLine($"Message Status After MarkAsSending: {message.Status}");

var sendResult = SendResult.Success(
    providerName: ProviderName.ProviderA,
    description: "ProviderA simulated success.");

message.MarkAsSent();

Console.WriteLine($"Provider: {sendResult.ProviderName}");
Console.WriteLine($"Send Result: {sendResult.IsSuccess}");
Console.WriteLine($"Final Status: {message.Status}");