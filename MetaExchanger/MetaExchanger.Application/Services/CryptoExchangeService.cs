using FluentValidation;
using MetaExchanger.Application.Common;
using MetaExchanger.Application.Domain;
using MetaExchanger.Application.Infrastructure;

namespace MetaExchanger.Application.Services
{
    /// <inheritdoc cref="ICryptoExchangeService"/>
    public class CryptoExchangeService : ICryptoExchangeService
    {
        private readonly IValidator<DomainOrder> _orderValidator;
        private readonly ApplicationDbContext _dbContext;
        private readonly IOrderService _orderService;

        public CryptoExchangeService(IValidator<DomainOrder> orderValidator, ApplicationDbContext dbContext, IOrderService orderService)
        {
            _orderValidator = orderValidator;
            _dbContext = dbContext;
            _orderService = orderService;
        }

        public async Task<Result<IEnumerable<DomainOrder>>> GetBestExecutionAsync(DomainOrder domainOrder, CancellationToken token = default)
        {
            await _orderValidator.ValidateAndThrowAsync(domainOrder, cancellationToken: token);

            var responseResult = new List<DomainOrder>();

            if (domainOrder.Type == OperationType.Sell)
            {
                var result = await _orderService.ProcessAsSell(domainOrder, responseResult);
                if (!result.Ok)
                    return result.Error!;
            }
            else
            {
                var result = await _orderService.ProcessAsBuy(domainOrder, responseResult);
                if (!result.Ok)
                    return result.Error!;
            }

            await _dbContext.SaveChangesAsync(token);
            return responseResult;
        }
    }
}