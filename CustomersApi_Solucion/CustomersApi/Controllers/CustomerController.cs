using CustomersApi.CasosDeUso;
using CustomersApi.Dtos;
using CustomersApi.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static CustomersApi.Repositories.CustomerDatabaseContext;

namespace CustomersApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : Controller
    {
        private readonly CustomerDatabaseContext _customerDatabaseContext;
        private readonly IUpdateCustomerUseCase _updateCustomerUseCase;

        public CustomerController(CustomerDatabaseContext customerDatabaseContext, IUpdateCustomerUseCase updateCustomerUseCase)
        {
            _customerDatabaseContext = customerDatabaseContext;
            _updateCustomerUseCase = updateCustomerUseCase;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<CustomerDto>))]
        public async Task<IActionResult> GetCustomers()
        {
            var result = _customerDatabaseContext.Customer
                .Select(c => c.ToDto())
                .ToList();

            return new OkObjectResult(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CustomerDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCustomer(long id)
        {
            var result = await _customerDatabaseContext.Get(id);

            if (result == null)
                return new NotFoundResult();

            return new OkObjectResult(result.ToDto());
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        public async Task<IActionResult> DeleteCustomer(long id)
        {
            var result = await _customerDatabaseContext.Delete(id);
            return new OkObjectResult(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CustomerDto))]
        public async Task<IActionResult> CreateCustomer(CustomerCreateDto customer)
        {
            var result = await _customerDatabaseContext.Add(customer);
            return new CreatedResult($"https://localhost:7211/api/customer/{result.Id}", result.ToDto());
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CustomerDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateCustomer(CustomerDto customer)
        {
            var result = await _updateCustomerUseCase.Execute(customer);
            if (result == null)
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(result);
        }
    }
}
