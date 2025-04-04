using MetaExchanger.Application.Common;
using MetaExchanger.Application.Models;

namespace MetaExchanger.Application.Domain
{
    public static class Mapper
    {
        public static DomainOrder Convert(this Order order)
        {
            return new DomainOrder()
            {
                Id = order.Id,
                Amount = order.Amount,
                CryptoExchangeId = order.CryptoExchangeId,
                Kind = Kind.Limit,
                Price = order.Price,
                Time = order.Time ?? DateTime.Now,
                Type = OperationType.Buy
            };
        }

        public static Order Convert(this DomainOrder order)
        {
            return new Order()
            {
                Id = order.Id,
                Amount = order.Amount,
                CryptoExchangeId = order.CryptoExchangeId!.Value,
                Kind = "Limit",
                Price = order.Price!.Value,
                Time = order.Time,
                Type = order.Type == OperationType.Buy ? "Buy" : "Sell",
            };
        }
    }
}