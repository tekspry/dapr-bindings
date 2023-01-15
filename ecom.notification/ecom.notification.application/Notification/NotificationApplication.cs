using ecom.notification.domain.Order;
using ecom.notification.infrastructure.Services.Customer;
using ecom.notification.infrastructure.Services.Product;
using Dapr.Client;
using Microsoft.Extensions.Logging;
using ecom.notification.domain.Notification;
using ecom.notification.infrastructure.Services.Email;
using ecom.notification.domain.Customer;

namespace ecom.notification.application.Notification
{
    public class NotificationApplication : INotificationApplication
    {
        private readonly IProductService _productService;
        private readonly ICustomerService _customerService;
        private readonly IEmailService _emailService;
        private readonly DaprClient _daprClient;
        private readonly ILogger<NotificationApplication> _logger;

        public NotificationApplication(IProductService productService, ICustomerService customerService, IEmailService emailService, DaprClient daprClient, ILogger<NotificationApplication> logger)
        {
            this._productService = productService;
            this._customerService = customerService;
            this._daprClient = daprClient;
            this._emailService = emailService;
            this._logger = logger;
        }
        public async Task SendNotificationAsync(Order order)
        {
            if (string.IsNullOrEmpty(order.CustomerId))
                order.CustomerId = "0787e3c6-5886-4c73-bbef-81f5bf79967b";

            var customerDetails = await _customerService.GetCustomerAsync(order.CustomerId);

            //var customerDetails = new ecom.notification.domain.Customer.CustomerDetails()
            //{
            //    Id = "0787e3c6-5886-4c73-bbef-81f5bf79967b",
            //    Name = "TestUser1",
            //    Email = "gagan1983@gmail.com",
            //    ContactNumber = 1111111111,
            //    Address = new CustomerAddress()
            //    {
            //        Address = "test addr 1",
            //        City = "New Delhi",
            //        PostalCode = "123xyz",
            //        Country = "India"
            //    }
            //};
            var productDetails = await _productService.GetProductAsync(order.ProductId);

            var orderForNotification = new OrderForNotfication(order, customerDetails, productDetails);

            _logger.LogInformation($"Invoking SendEmailForOrder from Notification application class");
            await _emailService.SendEmailForOrder(orderForNotification);
        }
    }
}
