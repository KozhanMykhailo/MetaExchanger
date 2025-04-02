using MetaExchanger.Application.Services;
using MetaExchanger.Contracts.Requests;
using MetaExchanger.Contracts.Responses;
using MetaExchanger.Api.Mapping;
using Microsoft.AspNetCore.Mvc;

namespace MetaExchanger.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class OrdersController : Controller
    {
        private readonly ICryptoExchangeService _CrExService;

        public OrdersController(ICryptoExchangeService crExService)
        {
            _CrExService = crExService;
        }

        /// <summary>
        /// OperationType enum values: 0 - Buy, 1 - Sell
        /// </summary>
        /// <param name="request">Request object</param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(OrderResponce), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationFailureResponce), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request, CancellationToken token)
        {
            var domainOrder = request.ToDomainOrder();
            var result =  await _CrExService.CreateAsync(domainOrder, token);
            if (!result.Ok)
                return BadRequest(result.Error);

            var responce = result.Value.ToOrderResponce();
            return Created(string.Empty, responce);
        }
    }
}
