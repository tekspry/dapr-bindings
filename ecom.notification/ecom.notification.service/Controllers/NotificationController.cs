using Dapr;
using ecom.notification.application.Notification;
using ecom.notification.domain.Order;
using Microsoft.AspNetCore.Mvc;

namespace ecom.notification.service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationApplication _NotificationApplication;
        private readonly ILogger<NotificationController> logger;
        public NotificationController(INotificationApplication NotificationApplication, ILogger<NotificationController> logger)
        {
            _NotificationApplication = NotificationApplication;
            this.logger = logger;
        }
        [HttpPost("", Name = "SubmitOrder")]
        [Topic("orderpubsub", "payments")]
        public async Task<IActionResult> Submit(Order order)
        {
            //logger.LogInformation($"Notification service received for new order: {order.OrderId} message from payment service");
            //logger.LogInformation($"Order Details --> Product: {order.ProductId}, Product Quantity: {order.ProductCount}, Price: {order.OrderPrice}");

            logger.LogInformation($"Notification service received for new order: {order.OrderId} message from payment service");
            logger.LogInformation($"Notification Controller invoking notification application");
            await _NotificationApplication.SendNotificationAsync(order);

            return Ok();
        }
    }
}
