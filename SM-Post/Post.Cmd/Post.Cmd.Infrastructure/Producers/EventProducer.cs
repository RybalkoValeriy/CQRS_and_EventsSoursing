using System.Text.Json;
using Confluent.Kafka;
using CQRS.Core.Events;
using CQRS.Core.Producer;

namespace Post.Cmd.Infrastructure.Producers;

public class EventProducer : IEventProducer
{
    private readonly ProducerConfig _config;

    public EventProducer()
    {
        var cfg = new ProducerConfig
        {
            BootstrapServers = "localhost:9092"
        };

        _config = cfg;
    }

    public async Task ProduceAsync<T>(string topic, T @event) where T : BaseEvent
    {
        using var producer = new ProducerBuilder<string, string>(_config)
        .SetKeySerializer(Serializers.Utf8)
        .SetValueSerializer(Serializers.Utf8)
        .Build();

        var message = new Message<string, string>
        {
            Key = Guid.NewGuid().ToString(),
            Value = JsonSerializer.Serialize(@event, @event.GetType())
        };

        var deliveryResult = await producer.ProduceAsync(topic, message);

        if (deliveryResult.Status == PersistenceStatus.NotPersisted)
            throw new Exception(
                $"Could not produce {@event.GetType().Name} message to topic - {topic} reason:{deliveryResult.Message}"
                );

    }
}
