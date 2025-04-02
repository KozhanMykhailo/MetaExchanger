using MetaExchanger.Application.Common;
using MetaExchanger.Application.Domain;
using MetaExchanger.Contracts.Requests;

namespace MetaExchanger.Api.Mapping
{
    public static class ContractToDomainMapper
    {
        public static DomainOrder ToDomainOrder(this GetOrdersRequest request)
        {
            return new DomainOrder
            {
                Id = Guid.NewGuid(),
                Time = DateTime.Now,
                Type = request.OperationType,
                Kind = Kind.Limit,
                Amount = request.Amount
            };
        }
    }
}
