using Dapr.Client;
using ecom.order.database.order;
using ecom.order.domain.Order;
using ecom.order.infrastructure.Product;
using Microsoft.Extensions.Logging;

namespace ecom.order.application.Order
{
    public class OrderApplication : IOrderApplication
    {
        private readonly IProductService _productService;
        private readonly IOrderRepository _orderRepository;
        private readonly DaprClient _daprClient;
        private readonly ILogger<OrderApplication> logger;

        public OrderApplication(IProductService productService, IOrderRepository orderRepository, DaprClient daprClient, ILogger<OrderApplication> logger)
        {
            _productService = productService;
            _orderRepository = orderRepository;
            this._daprClient = daprClient;
            this.logger = logger;
        }

        public async Task<IEnumerable<domain.Order.Order>> ListAsync() => await _orderRepository.GetOrders();

        public async Task<IEnumerable<domain.Order.OrderDetails>> ListOrderDetailsAsync() => await _orderRepository.GetOrderDetails();

        public async Task<domain.Order.Order> AddAsync(domain.Order.Order order)
        {   
            var productPrice = await _productService.UpdateProductQuantity(order.ProductId, order.ProductCount);
            order.OrderId = Guid.NewGuid().ToString();            
            order.OrderState = OrderState.OrderPlaced;
            order.OrderPlacedAt = DateTime.UtcNow;

            await _orderRepository.CreateOrder(order);
            
            await _daprClient.PublishEventAsync("orderpubsub", "orders", order);
            
            order.OrderState = OrderState.OrderPaymentPending;
            await _orderRepository.UpdateAsync(order); ;

            return order;
        }

        public async Task<domain.Order.Order> GetAsync(string id)
        {
            return await _orderRepository.GetOrderById(id);
        }

        public async Task UpdateAsync(domain.Order.Order order) => await _orderRepository.UpdateAsync(order);

        public async Task<int> UpdateOrderPaymentPending()
        {
            var orders = await _orderRepository.GetOrders();
            var pendingPaymentOrders = orders.Where(o => o.OrderState == OrderState.OrderPaymentPending).ToList();
            
            var orderProcessed = pendingPaymentOrders.Count;
            if (orderProcessed > 0)
            {
                foreach (var order in pendingPaymentOrders)
                {
                    TimeSpan ts = DateTime.UtcNow - order.OrderPlacedAt;

                    if (ts.TotalMinutes > 1)
                    {
                        order.OrderState = OrderState.OrderPaymentExpired;
                        await _orderRepository.UpdateAsync(order);
                        var productPrice = await _productService.UpdateProductQuantity(order.ProductId, -order.ProductCount);
                    }
                }
            }
            return orderProcessed;
        }

        public async Task DeleteAsync(string id) => await _orderRepository.DeleteAsync(id);
        public async Task DeleteAllAsync() => await _orderRepository.DeleteAllAsync(); 

    }
}
