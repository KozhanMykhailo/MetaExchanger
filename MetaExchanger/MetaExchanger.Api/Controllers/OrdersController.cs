using MetaExchanger.Application.Services;
using MetaExchanger.Contracts.Requests;
using MetaExchanger.Contracts.Responses;
using MetaExchanger.Api.Mapping;
using Microsoft.AspNetCore.Mvc;
using MetaExchanger.Application.Common;

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
        /// /// api/v1/orders?OperationType=Buy&Amount=10
        [HttpGet]
        [ProducesResponseType(typeof(OrderResponce), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationFailureResponce), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateOrder(OperationType OperationType, decimal Amount, CancellationToken token)
        {
            var request = new GetOrdersRequest() { Amount = Amount , OperationType = OperationType };
            var domainOrder = request.ToDomainOrder();
            var result =  await _CrExService.GetOrdersAsync(domainOrder, token);
            if (!result.Ok)
                return BadRequest(result.Error);

            var responce = result.Value.ToOrderResponce();
            return Ok(responce);
        }
    }
}
