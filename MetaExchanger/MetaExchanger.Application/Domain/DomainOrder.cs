using MetaExchanger.Application.Common;

namespace MetaExchanger.Application.Domain
{
    /// <summary>
    /// Domain entity, between dbContext and response(dto).
    /// </summary>
    public class DomainOrder
    {
        public required Guid Id { get; init; }

        public Guid? CryptoExchangeId { get; set; }

        public required DateTime Time { get; init; }

        public required OperationType Type { get; init; }

        public required Kind Kind { get; init; }

        public decimal? Price { get; set; }

        public required decimal Amount { get; set; }
    }
}