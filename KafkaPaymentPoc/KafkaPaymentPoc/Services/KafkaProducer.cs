using Confluent.Kafka;
using System.Text.Json;

namespace KafkaPaymentPoc.Services
{
    public class KafkaProducer
    {
        private readonly string _bootstrapServers = "localhost:9092";

        private readonly string _topic = "payment-success";

        public async Task PublishAsync<T>(T message)
        {
            var config = new ProducerConfig
            {
                BootstrapServers = _bootstrapServers
            };

            using var producer =
                new ProducerBuilder<Null, string>(config).Build();

            var json = JsonSerializer.Serialize(message);

            var kafkaMessage = new Message<Null, string>
            {
                Value = json
            };

            var result = await producer.ProduceAsync(
                _topic,
                kafkaMessage);

            Console.WriteLine(
                $"Message delivered to {result.TopicPartitionOffset}");
        }
    }
}
