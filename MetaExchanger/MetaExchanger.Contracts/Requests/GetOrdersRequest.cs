using MetaExchanger.Application.Common;

namespace MetaExchanger.Contracts.Requests
{
    public class GetOrdersRequest
    {
        /// <summary>
        /// Operation type.
        /// </summary>
        public required OperationType OperationType { get; init; }

        /// <summary>
        /// Amount of BTC.
        /// </summary>
        public required decimal Amount { get; init; }
    }
}