using MetaExchanger.Application.Common;

namespace MetaExchanger.Contracts.Responses
{
    public class OrderResponce
    {
        //public required Guid Id { get; init; }

        //public required DateTime Time { get; init; }

        //public required OperationType Type { get; init; }

        //public required Kind Kind { get; init; }

        public required decimal TotalPrice { get; init; }

        //public required double Amount { get; init; }

        public required string BestExecutionResponce { get; init; }
    }
}