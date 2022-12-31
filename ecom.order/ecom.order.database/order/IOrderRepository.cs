using ecom.order.domain.Order;

namespace ecom.order.database.order
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetOrders();
        Task<Order> CreateOrder(Order order);
        Task<Order> GetOrderById(string orderId);
        Task UpdateAsync(Order order);
    }
}
