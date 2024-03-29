﻿namespace ecom.order.domain.Order
{
    public enum OrderState
    {
        OrderPlaced = 1,
        OrderClosed = 2,
        OrderConfirmed = 3,
        OrderCanceled = 4,
        OrderShipped = 5,
        OrderReturned = 6,
        OrderPaymentPending = 7,
        OrderPaymentRejected = 8,
        OrderPaymentConfirmed = 9,
        OrderPaymentExpired = 10,
    }
}
