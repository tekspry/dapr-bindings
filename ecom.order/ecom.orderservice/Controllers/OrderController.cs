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

        [HttpGet(Name = "GetOrders")]
        public async Task<IEnumerable<OrderDetails>> GetAll() => await _orderApplication.ListOrderDetailsAsync();

        [HttpPut(Name = "UpdateOrder")]
        public async Task Update(Order order) => await _orderApplication.UpdateAsync(order);

        [HttpDelete("{id}", Name = "DeleteById")]
        public async Task DeleteById(string id) => await _orderApplication.DeleteAsync(id);

        [HttpDelete(Name = "DeleteAll")]
        public async Task DeleteAll () => await _orderApplication.DeleteAllAsync();
    }
}
