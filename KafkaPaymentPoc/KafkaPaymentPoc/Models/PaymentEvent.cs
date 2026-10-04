namespace KafkaPaymentPoc.Models
{
    public class PaymentEvent
    {
        public string PaymentId { get; set; } = string.Empty;

        public string CustomerId { get; set; } = string.Empty;

        public decimal Amount { get; set; }

        public string Currency { get; set; } = "INR";

        public string Status { get; set; } = string.Empty;

        public DateTime Timestamp { get; set; }
    }
}
