using ecom.order.domain.Customers;

namespace ecom.order.domain.Order
{
    public record OrderDetails(Customer customerDetails, IEnumerable<Order> orders);
    
}
