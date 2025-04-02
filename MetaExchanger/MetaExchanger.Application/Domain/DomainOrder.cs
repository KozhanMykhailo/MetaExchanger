using MetaExchanger.Application.Common;
using MetaExchanger.Application.Models;

namespace MetaExchanger.Application.Domain
{
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