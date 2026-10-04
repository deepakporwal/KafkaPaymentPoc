using KafkaPaymentPoc.Models;
using KafkaPaymentPoc.Services;
using Microsoft.AspNetCore.Mvc;

namespace KafkaPaymentPoc.Controllers
{
    [ApiController]
    [Route("api/payments")]
    public class PaymentsController : ControllerBase
    {
        private readonly KafkaProducer _kafkaProducer;

        public PaymentsController(KafkaProducer kafkaProducer)
        {
            _kafkaProducer = kafkaProducer;
        }

        [HttpPost]
        public async Task<IActionResult> MakePayment(
            PaymentEvent payment)
        {
            payment.Status = "SUCCESS";
            payment.Timestamp = DateTime.UtcNow;

            await _kafkaProducer.PublishAsync(payment);

            return Ok(new
            {
                message = "Payment successful",
                paymentId = payment.PaymentId
            });
        }
    }
}
