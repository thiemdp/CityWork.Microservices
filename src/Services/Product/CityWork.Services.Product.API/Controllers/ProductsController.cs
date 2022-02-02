using Microsoft.AspNetCore.Mvc;

namespace CityWork.Services.Product.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly Dispatcher _dispatcher;
        public ProductsController(Dispatcher dispatcher,ILogger<ProductsController> logger)
        {
            _dispatcher = dispatcher;
            _logger = logger;
        }

        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult< ProductFullResponse>> Post([FromBody] AddUpdateProductCommand input)
        {
            var response = await _dispatcher.DispatchAsync (input);
            return Ok(response);
        }

        [HttpGet("[action]")]
        [Consumes("application/json")]
        public async Task<ActionResult<ProductFullResponse>> GetById([FromQuery] GetProductByIdQuery input)
        {
            var response = await _dispatcher.DispatchAsync(input);
            return Ok(response);
        }

        [HttpGet("[action]")]
        [Consumes("application/json")]
        public async Task<ActionResult<PagedResult<ProductFullResponse>>> GetAll([FromQuery] GetProductPagedAndSortedQuery input)
        {
            var response = await _dispatcher.DispatchAsync(input);
            return Ok(response);
        }

        [HttpDelete("[action]")]
        [Consumes("application/json")]
        public async Task<ActionResult> Delete([FromQuery] DeleteProductCommand input)
        {
              await _dispatcher.DispatchAsync(input);
            return Ok();
        }
    }
}
