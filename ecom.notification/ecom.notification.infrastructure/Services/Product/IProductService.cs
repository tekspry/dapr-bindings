namespace ecom.notification.infrastructure.Services.Product
{
    public interface IProductService
    {
        Task<ecom.notification.domain.Product.Product> GetProductAsync(string productId);
    }
}
