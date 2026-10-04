using Confluent.Kafka;

namespace Payment.Worker;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;

    private readonly ConsumerConfig _config;

    public Worker(ILogger<Worker> logger)
    {
        _logger = logger;

        _config = new ConsumerConfig
        {
            BootstrapServers = "localhost:9092",

            GroupId = "payment-worker-group",

            AutoOffsetReset = AutoOffsetReset.Earliest,

            EnableAutoCommit = true
        };
    }

    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken)
    {
        using var consumer =
            new ConsumerBuilder<Ignore, string>(_config)
                .Build();

        consumer.Subscribe("payment-success");

        _logger.LogInformation(
            "Payment Worker started...");

        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var result = consumer.Consume(
                    stoppingToken);

                _logger.LogInformation(
                    "Payment event received: {Message}",
                    result.Message.Value);
            }
        }
        catch (OperationCanceledException)
        {
            consumer.Close();
        }
    }
}