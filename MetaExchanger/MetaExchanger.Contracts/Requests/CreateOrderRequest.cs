using MetaExchanger.Application.Common;

namespace MetaExchanger.Contracts.Requests
{
    public class CreateOrderRequest
    {
        /// <summary>
        /// Operation type.
        /// </summary>
        public required OperationType OperationType { get; init; }

        /// <summary>
        /// Amount of BTC.
        /// </summary>
        public required decimal Amount { get; init; }
        /*
        /// <summary>
        /// Required if it's selling.
        /// </summary>
        public required Guid CryptoExchangeId { get; init; }
        */
    }
}