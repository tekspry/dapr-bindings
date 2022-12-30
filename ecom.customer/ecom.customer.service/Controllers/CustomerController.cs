using ecom.customer.application.Customer;
using ecom.customer.domain.Customer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ecom.customer.service.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerApplication _customerApplication;

        private readonly ILogger<CustomerController> _logger;

        public CustomerController(ICustomerApplication customerApplication, ILogger<CustomerController> logger)
        {
            _customerApplication = customerApplication;
            _logger = logger;
        }

        [HttpGet(Name = "GetCustomers")]
        public async Task<IEnumerable<CustomerDetails>> GetAll() => await _customerApplication.ListAsync();


        [HttpGet("{id}", Name = "GetById")]
        public async Task<CustomerDetails> GetById(string id) => await _customerApplication.GetAsync(id);


        [HttpPost(Name = "AddCustomer")]
        public async Task<string> Add(CustomerDetails product) => await _customerApplication.AddAsync(product);

        [HttpPut(Name = "UpdateCustomer")]
        public async Task<string> Update(CustomerDetails product) => await _customerApplication.UpdateAsync(product);
    }
}
