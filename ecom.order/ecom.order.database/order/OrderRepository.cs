using Dapr.Client;
using ecom.order.domain.Order;
using Microsoft.Extensions.Logging;

namespace ecom.order.database.order
{
    public class OrderRepository : IOrderRepository
    {
        private List<Order> orders = new List<Order>();
        private readonly DaprClient daprClient;
        private readonly ILogger<OrderRepository> logger;
        private const string cacheStoreName = "ordercache";

        public OrderRepository(DaprClient daprClient, ILogger<OrderRepository> logger)
        {
            this.daprClient = daprClient;
            this.logger = logger;
        }

        public async Task<IEnumerable<Order>> GetOrders()
        {
            var key = $"orderlist";
            var orderList = await daprClient.GetStateAsync<List<Order>>(cacheStoreName, key);

            return orderList;
        }
        public async Task<Order> CreateOrder(Order order)
        {
            order.OrderId = Guid.NewGuid().ToString();
            var key = $"orderlist";
            var orders = await daprClient.GetStateAsync<List<Order>>(cacheStoreName, key);

            if (orders == null)
                orders = new List<Order>();
                       
            orders.Add(order);
            
            await this.SaveOrderListToCacheStore(orders);

            return await Task.FromResult(order);
        }

        public async Task<Order> GetOrderById(string orderId)
        {
            var key = $"orderlist";
            var orders = await daprClient.GetStateAsync<List<Order>>(cacheStoreName, key);
            var @order = orders.FirstOrDefault(e => e.OrderId == orderId);
            if (@order == null)
            {
                throw new InvalidOperationException("order not found");
            }
            return @order;
        }
        public async Task UpdateAsync(Order order)
        {
            var key = $"orderlist";
            var orders = await daprClient.GetStateAsync<List<Order>>(cacheStoreName, key);

            var updatedOrder = orders.Where(x => x.OrderId == order.OrderId).FirstOrDefault();

            if (updatedOrder != null)
            {
                orders.Remove(updatedOrder);

                orders.Add(order);
            }

            await this.SaveOrderListToCacheStore(orders);
        }
        private async Task SaveOrderListToCacheStore(List<Order> orders)
        {
            var key = $"orderlist";
            await daprClient.SaveStateAsync(cacheStoreName, key, orders);
            logger.LogInformation($"Created new order in cache store {cacheStoreName}");
        }
    }
}
