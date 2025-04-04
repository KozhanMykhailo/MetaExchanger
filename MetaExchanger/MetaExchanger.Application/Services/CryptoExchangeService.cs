using FluentValidation;
using MetaExchanger.Application.Common;
using MetaExchanger.Application.Domain;
using MetaExchanger.Application.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace MetaExchanger.Application.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class CryptoExchangeService : ICryptoExchangeService
    {
        private readonly IValidator<DomainOrder> _orderValidator;
        private readonly ApplicationDbContext _dbContext;

        public CryptoExchangeService(IValidator<DomainOrder> orderValidator, ApplicationDbContext dbContext)
        {
            _orderValidator = orderValidator;
            _dbContext = dbContext;
        }

        public async Task<Result<IEnumerable<DomainOrder>>> GetOrdersAsync(DomainOrder domainOrder, CancellationToken token = default)
        {
            await _orderValidator.ValidateAndThrowAsync(domainOrder, cancellationToken: token);

            var responseResult = new List<DomainOrder>();

            if (domainOrder.Type == OperationType.Sell)
            {
                var result = await ProcessAsSell(domainOrder, responseResult);
                if (!result.Ok)
                    return result.Error!;
            }
            else
            {
                var result = await ProcessAsBuy(domainOrder, responseResult);
                if (!result.Ok)
                    return result.Error!;
            }

            await _dbContext.SaveChangesAsync(token);
            return responseResult;
        }

        private async Task<Result> ProcessAsBuy(DomainOrder requestDomainOrder, List<DomainOrder> responseResult)
        {
            var cryptoExchangeStates = new Dictionary<Guid, decimal>();

            var remainingAmountBuy = requestDomainOrder.Amount;
            var allOrders = await _dbContext.Orders.Include(o => o.CryptoExchange)
                .Where(o => o.Type == "Sell")
                .OrderBy(o => o.Price).ThenBy(o => o.Time)
                .ToListAsync();

            foreach (var order in allOrders) 
            {
                //check meta balance during current request handling
                if (cryptoExchangeStates.TryGetValue(order.CryptoExchangeId, out decimal btcAvailabel))
                {
                    //check - is it enough BTC balance of some CryptoExchange to make some operation with bids(type 'Sell')
                    if (btcAvailabel < order.Amount && btcAvailabel < remainingAmountBuy) continue; 
                }

                //if it's first iteration and CryptoExchange has enough btc balance - return as good deal
                if (order.Amount >= remainingAmountBuy)
                {
                    var domainOrder = order.Convert();
                    //if CryptoExchange has Bid('Sell' with btc balance > buyAmountRemaining) but does not have enough btc balance
                    if (!cryptoExchangeStates.ContainsKey(order.CryptoExchangeId) && order.CryptoExchange.BalanceBTC < remainingAmountBuy)
                    {
                        //store temporary state of CryptoExchange BTC balance
                        //take MAX BTC from CryptoExchange, in this case take all available balance
                        cryptoExchangeStates.Add(order.CryptoExchangeId, 0);
                        var order1 = order.Convert();
                        order1.Amount = order.CryptoExchange.BalanceBTC;
                        responseResult.Add(order1);
                        remainingAmountBuy -= order.CryptoExchange.BalanceBTC;
                        LogToConsole(order1);
                        continue;
                    }

                    domainOrder.Amount = remainingAmountBuy;
                    remainingAmountBuy = 0;

                    responseResult.Add(domainOrder);
                    LogToConsole(domainOrder);
                    break;
                }

                //store temporary state of CryptoExchange BTC balance
                //first add occures with '- order.Amount'
                if (!cryptoExchangeStates.TryAdd(order.CryptoExchangeId, order.CryptoExchange.BalanceBTC - order.Amount))
                {
                    var currentBtcValue = cryptoExchangeStates.GetValueOrDefault(order.CryptoExchangeId);
                    cryptoExchangeStates[order.CryptoExchangeId] = currentBtcValue - order.Amount;
                }

                remainingAmountBuy -= order.Amount;
                var suggestedOrder = order.Convert();

                responseResult.Add(suggestedOrder);
                LogToConsole(suggestedOrder);
            }

            if (remainingAmountBuy > 0)
                return new Error($"CryptoExchanges don't have enough BTC for dealing, not enough: {remainingAmountBuy} BTC");

            return Result.Empty;
        }

        private async Task<Result> ProcessAsSell(DomainOrder domainOrder, List<DomainOrder> responseResult)
        {
            var sellAmount = domainOrder.Amount;
            var order = await _dbContext.Orders.Include(o => o.CryptoExchange)
                .Where(o => o.Type == "Buy" && o.CryptoExchange.BalanceEUR >= (o.Price * sellAmount))
                .OrderByDescending(o => o.Price)
                .FirstOrDefaultAsync();

            if (order is null)
                return new Error("Order with the highest price not found");

            domainOrder.Price = order.Price;
            domainOrder.CryptoExchangeId = order.CryptoExchange.Id;
            //just create new order in DB
            var newOrder = domainOrder.Convert();

            _dbContext.Orders.Add(newOrder);

            responseResult.Add(domainOrder);
            LogToConsole(domainOrder);
            return Result.Empty;
        }

        private void LogToConsole(DomainOrder order)
        {
            if (order.Type == OperationType.Sell)
                Console.WriteLine($"\nThe best price for selling\nPrice: '{order.Price}', CryptoExchange ID: '{order.Id}'\n");
            else
                Console.WriteLine($"\nSuggested to Buy next:\nPrice: '{order.Price}', Amount: '{order.Amount}' CryptoExchange ID: '{order.Id}'\n");
        }
    }
}