using MetaExchanger.Application.Common;
using MetaExchanger.Application.Domain;
using MetaExchanger.Contracts.Requests;

namespace MetaExchanger.Api.Mapping
{
    public static class ContractToDomainMapper
    {
        public static DomainOrder ToDomainOrder(this CreateOrderRequest request)
        {
            return new DomainOrder
            {
                Id = Guid.NewGuid(),
                Time = DateTime.Now,
                //CryptoExchangeId = request.CryptoExchangeId,
                Type = request.OperationType,
                Kind = Kind.Limit,
                Amount = request.Amount
            };
        }
    }
}
