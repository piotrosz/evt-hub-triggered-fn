using Azure.Identity;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using System.Text;

// number of events to be sent to the event hub
const int numOfEvents = 3;

// The Event Hubs client types are safe to cache and use as a singleton for the lifetime
// of the application, which is best practice when events are being published or read regularly.

var producerClient = new EventHubProducerClient(
    "evthubtest.servicebus.windows.net",
    "test-evhb",
    new DefaultAzureCredential());

// Create a batch of events 
using EventDataBatch eventBatch = await producerClient.CreateBatchAsync();

for (int eventIndex = 1; eventIndex <= numOfEvents; eventIndex++)
{
    var eventData = new EventData(Encoding.UTF8.GetBytes($"Event {eventIndex}"))
    {
        Properties =
        {
            ["EventType"] = eventIndex == numOfEvents - 1 ? "add" : "other"
        }
    };

    if (!eventBatch.TryAdd(eventData))
    {
        // if it is too large for the batch
        throw new Exception($"Event {eventIndex} is too large for the batch and cannot be sent.");
    }
}

try
{
    // Use the producer client to send the batch of events to the event hub
    await producerClient.SendAsync(eventBatch);
    Console.WriteLine($"A batch of {numOfEvents} events has been published.");
    Console.ReadLine();
}
finally
{
    await producerClient.DisposeAsync();
}