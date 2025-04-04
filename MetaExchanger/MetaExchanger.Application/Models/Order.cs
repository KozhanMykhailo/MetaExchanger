using MetaExchanger.Application.Common;

namespace MetaExchanger.Application.Models
{
    public class Order
    {
        public required Guid Id { get; init; }

        public required DateTime? Time { get; init; }

        public required string Type { get; init; }

        public required string Kind { get; init; }

        public required decimal Price { get; set; }

        public required decimal Amount { get; set; }

        public required Guid CryptoExchangeId { get; init; }
        public CryptoExchange CryptoExchange { get; set; }
    }
}