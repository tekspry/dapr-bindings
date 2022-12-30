using ecom.notification.domain.Order;

namespace ecom.notification.application.Notification
{
    public interface INotificationApplication
    {
        Task<Order> SendAsync(Order order);
    }
}
