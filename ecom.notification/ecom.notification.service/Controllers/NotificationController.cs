﻿using Dapr;
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
            await _NotificationApplication.SendNotificationAsync(order);

            return Ok();
        }
    }
}
