using System.Text.Json;
using Confluent.Kafka;
using CQRS.Core.Consumers;
using CQRS.Core.Events;
using Microsoft.Extensions.Options;
using Post.Query.Infrastructure.Converters;
using Post.Query.Infrastructure.Handlers;

namespace Post.Query.Infrastructure.Consumers;

public class EventConsumer(
    IOptions<ConsumerConfig> config,
    IEventHandlerOld eventHandlerOld)
    : IEventConsumer
{
    private readonly ConsumerConfig _config = config.Value;

    public void Consume(string topic)
    {
        using var consumer = new ConsumerBuilder<string, string>(_config)
                .SetKeyDeserializer(Deserializers.Utf8)
                .SetValueDeserializer(Deserializers.Utf8)
                .Build();

        consumer.Subscribe(topic);

        while (true)
        {
            var consumeResult = consumer.Consume();

            if (consumeResult?.Message is null) continue;

            var options = new JsonSerializerOptions { Converters = { new EventJsonConverter() } };
            var @event = JsonSerializer.Deserialize<BaseEvent>(consumeResult.Message.Value, options);
            var handlerMethod = eventHandlerOld.GetType().GetMethod("On", new[]{ @event.GetType() });

            if (handlerMethod is null)
            {
                throw new ArgumentNullException(nameof(handlerMethod), "Could not find event handler method!");
            }

            handlerMethod.Invoke(eventHandlerOld, new object[] { @event });
            consumer.Commit(consumeResult);
        }
    }
}
