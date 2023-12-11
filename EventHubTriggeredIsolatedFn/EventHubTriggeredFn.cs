using Azure.Messaging.EventHubs;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace EventHubTriggeredIsolatedFn;

public class EventHubTriggeredFn
{
    private readonly ILogger<EventHubTriggeredFn> _logger;

    public EventHubTriggeredFn(ILogger<EventHubTriggeredFn> logger)
    {
        _logger = logger;
    }

    [Function(nameof(EventHubTriggeredFn))]
    public void Run([EventHubTrigger("test-evhb", Connection = "EventHubConnectionString")] EventData[] events)
    {
        foreach (EventData @event in events)
        {
            var enqueuedTime = @event.EnqueuedTime;
            var sequenceNumber = @event.SequenceNumber;

            _logger.LogInformation("EnqueuedTime: {enqueuedTime}, SequenceNumber: {sequenceNumber}", enqueuedTime, sequenceNumber);

            var partitionKey = @event.PartitionKey;
            var offset = @event.Offset;
            var messageId = @event.MessageId;
            _logger.LogInformation("PartitionKey: {partitionKey}, Offset: {offset}, MessageId: {messageId}", partitionKey, offset, messageId);

            var eventType = @event.Properties["EventType"].ToString();
            _logger.LogInformation("EventType is: {eventType}", eventType);

            if (eventType == "add")
            {
                _logger.LogWarning("Handling event {eventType}...", eventType);
                _logger.LogInformation("Handling event...");
                _logger.LogInformation("Event Body: {body}", @event.Body);
                _logger.LogInformation("Event Content-Type: {contentType}", @event.ContentType);
            }
            else
            {
                _logger.LogWarning("Skipping this event type...");
            }
        }
    }
}