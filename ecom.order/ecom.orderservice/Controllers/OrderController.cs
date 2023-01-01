using ecom.order.application.Order;
using ecom.order.domain.Order;
using Microsoft.AspNetCore.Mvc;

namespace ecom.order.service.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderApplication _orderApplication;

        private readonly ILogger<OrderController> _logger;

        public OrderController(IOrderApplication orderApplication, ILogger<OrderController> logger)
        {
            _orderApplication = orderApplication;
            _logger = logger;
        }

        [HttpPost(Name = "SubmitOrder")]
        public async Task<Order> Submit(Order order) => await _orderApplication.AddAsync(order);

        [HttpGet("{id}", Name = "GetById")]
        public async Task<Order> GetById(string id) => await _orderApplication.GetAsync(id);

        //[HttpPut(Name = "UpdateOrderPaymentPending")]
        //public async Task UpdateOrderPaymentPending()
        //{
        //    _logger.LogInformation("scheduled microservice UpdateOrderPaymentPending is called");
        //    await _orderApplication.UpdateOrderPaymentPending();
        //}

        [HttpGet(Name = "GetOrders")]
        public async Task<IEnumerable<OrderDetails>> GetAll() => await _orderApplication.ListOrderDetailsAsync();

        //[HttpGet("GetOrderDetails", Name = "GetOrderDetails")]
        //public async Task<IEnumerable<OrderDetails>> GetAllOrderDetails() => await _orderApplication.ListOrderDetailsAsync();

        [HttpPut(Name = "UpdateOrder")]
        public async Task Update(Order order) => await _orderApplication.UpdateAsync(order);
    }
}
