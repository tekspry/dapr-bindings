﻿using ecom.order.domain.Order;

namespace ecom.order.application.Order
{
    public interface IOrderApplication
    {
        Task<domain.Order.Order> AddAsync(domain.Order.Order order);
        Task<domain.Order.Order> GetAsync(string id);

        Task UpdateOrderPaymentPending();
    }
}
