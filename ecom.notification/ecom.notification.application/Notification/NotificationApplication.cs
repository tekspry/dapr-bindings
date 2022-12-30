using ecom.notification.domain.Order;

namespace ecom.notification.application.Notification
{
    public class NotificationApplication : INotificationApplication
    {
        public Task<Order> SendAsync(Order order)
        {
            throw new NotImplementedException();
        }
    }
}
