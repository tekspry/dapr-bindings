﻿using ecom.product.domain.Product;
using Dapr.Client;
using Microsoft.Extensions.Logging;

namespace ecom.product.database.ProductDB
{
    public class ProductRepository : IProductRepository
    {
        private List<Product> products = new List<Product>();
        private readonly DaprClient daprClient;
        private readonly ILogger<ProductRepository> logger;
        private const string cacheStoreName = "shoppingcache";

        public ProductRepository(DaprClient daprClient, ILogger<ProductRepository> logger)
        {
            this.daprClient = daprClient;
            this.logger = logger;

            LoadSampleData();            
        }
        public async Task<Product> GetProductById(string productId)
        {
            var products = await daprClient.GetStateAsync<List<Product>>(cacheStoreName, "productlist");
            var @product = products.FirstOrDefault(e => e.ProductId == productId);
            if (@product == null)
            {
                throw new InvalidOperationException("product not found");
            }
            return @product;
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            var productList = await daprClient.GetStateAsync<List<Product>>(cacheStoreName, "productlist");
            
            return productList;
        }

        public async Task<string> CreateProduct(Product product)
        {
            product.ProductId = Guid.NewGuid().ToString();
            var key = $"productlist";
            var products = await daprClient.GetStateAsync<List<Product>>(cacheStoreName, "productlist");

            products.Add(product);     
            await this.SaveProductListToCacheStore(products);

            return await Task.FromResult(product.ProductId);
        }

        public async Task<int> UpdateProduct(Product product)
        {            
            var key = $"productlist";
            var products = await daprClient.GetStateAsync<List<Product>>(cacheStoreName, "productlist");

            var updatedProduct = products.Where(x => x.ProductId == product.ProductId).FirstOrDefault();

            if (updatedProduct != null)
            {
                products.Remove(updatedProduct);

                products.Add(product);
            }

            await this.SaveProductListToCacheStore(products);            

            return product.Price;
        }


        private void LoadSampleData()
        {
            var TentHouseGuid = Guid.Parse("{b4312c9b-af56-4e6c-bc58-7c59cd6c5a6a}").ToString();
            var TableTennisGuid = Guid.Parse("{c9216eda-eaa8-4607-92d2-6e519653bda5}").ToString();
            var TravellingBagGuid = Guid.Parse("{59c687b6-a289-42b8-b04a-3e1e21bbc360}").ToString();

            products.Add(new Product
            {
                ProductId = TentHouseGuid,
                Name = "Tent House",
                Price = 899,
                AvailableSince = DateTime.Now.AddMonths(-6).ToString("MM/dd/yyyy"),
                ShortDescription = "Peppa pig theme play tent house for kids 5 years and above.",
                ImageUrl = "/tenthouse.jpg",
                Seller = "Toy Store",
                Quantity = 10
                
            });

            products.Add(new Product
            {
                ProductId = TableTennisGuid,
                Name = "Table Tennis",
                Price = 595,
                AvailableSince = DateTime.Now.AddMonths(-9).ToString("MM/dd/yyyy"),
                ShortDescription = "Table Tennis indoor/outdoor for adults and kids.",
                ImageUrl = "/tabletennis.png",
                Seller = "Sports Zone",
                Quantity = 20

            });

            products.Add(new Product
            {
                ProductId = TravellingBagGuid,
                Name = "Travelling Bag",
                Price = 499,
                AvailableSince = DateTime.Now.AddMonths(-2).ToString("MM/dd/yyyy"),
                ShortDescription = "Nylon 55 litres waterproof strolley Duffle Bag.",
                ImageUrl = "/travellingbag.png",
                Seller = "Bag Store",
                Quantity = 30

            });

            SaveProductListToCacheStore(products);
        }

        private async Task SaveProductListToCacheStore(List<Product> products)
        {
            var key = $"productlist";            
            await daprClient.SaveStateAsync(cacheStoreName, key, products);
            logger.LogInformation($"Created new product in cache store {key}");
        }
    }
}
