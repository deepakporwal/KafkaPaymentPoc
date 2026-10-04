# Kafka Payment POC

A small .NET proof-of-concept for processing payments asynchronously using Apache Kafka. The API accepts payment requests, publishes them to a Kafka topic, and a background worker consumes the messages and logs the payment details.

## Overview

This project demonstrates a simple event-driven payment flow:

1. A client sends a payment request to the ASP.NET Core API.
2. The API creates a `PaymentEvent` payload.
3. The payload is serialized and published to Kafka topic `payment-success`.
4. A separate background worker subscribes to the topic and consumes the message.
5. The worker logs the received message for downstream processing or auditing.

## Architecture

- `KafkaPaymentPoc/KafkaPaymentPoc` — ASP.NET Core API project
- `Payment.Worker` — Kafka consumer background service
- `PaymentEvent` model — payment payload used across producer and consumer

## Technologies

- .NET 10
- ASP.NET Core Web API
- Confluent.Kafka
- Apache Kafka
- Background worker pattern using `IHostedService`

## Prerequisites

Before running the project, ensure the following are installed:

- .NET SDK 10
- Apache Kafka broker running locally on `localhost:9092`
- If your Kafka setup differs, update the broker address in:
  - `KafkaPaymentPoc/KafkaPaymentPoc/Services/KafkaProducer.cs`
  - `Payment.Worker/Worker.cs`

## Project Structure

```text
KafkaPaymentPoc/
├── KafkaPaymentPoc/
│   └── KafkaPaymentPoc/
│       ├── Controllers/
│       │   └── PaymentsController.cs
│       ├── Models/
│       │   └── PaymentEvent.cs
│       ├── Services/
│       │   └── KafkaProducer.cs
│       ├── Program.cs
│       ├── KafkaPaymentPoc.csproj
│       └── appsettings.json
├── Payment.Worker/
│   ├── Worker.cs
│   ├── Program.cs
│   └── Payment.Worker.csproj
├── KafkaPaymentPoc.slnx
└── README.md
```

## Configuration

The application is configured to connect to Kafka at:

```text
localhost:9092
```

The producer publishes to topic:

```text
payment-success
```

The consumer subscribes to the same topic and listens for incoming payment events.

## Running the API

From the repository root, run:

```bash
dotnet restore
```

Then start the API:

```bash
dotnet run --project KafkaPaymentPoc/KafkaPaymentPoc/KafkaPaymentPoc.csproj
```

The API will run on the default ASP.NET Core port. You can send requests to:

```text
POST /api/payments
```

## Running the Worker

In a second terminal, start the consumer worker:

```bash
dotnet run --project Payment.Worker/Payment.Worker.csproj
```

This worker will remain active and listen for new payment messages from Kafka.

## Payment API Request

Example request body:

```json
{
  "paymentId": "PAY-1001",
  "customerId": "CUST-42",
  "amount": 499.99,
  "currency": "INR",
  "status": "PENDING"
}
```

The API automatically updates the event with:

- `status` = `SUCCESS`
- `timestamp` = current UTC time

## Example API Response

```json
{
  "message": "Payment successful",
  "paymentId": "PAY-1001"
}
```

## Message Flow Example

```text
Client -> POST /api/payments -> KafkaProducer.PublishAsync()
                                      -> payment-success topic
                                              -> Payment.Worker consumes message
                                                      -> logs payment event
```

## Notes

This repository is designed as a learning or proof-of-concept project for Kafka integration in .NET. The workflow is intentionally simple and can be extended with:

- persistent storage for payment records
- retry and dead-letter handling
- schema validation
- monitoring and metrics
- secure configuration via environment variables or secrets

## License

This project is currently provided as a sample/proof of concept without a formal license file.
