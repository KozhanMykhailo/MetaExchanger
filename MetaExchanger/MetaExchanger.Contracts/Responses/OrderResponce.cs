namespace MetaExchanger.Contracts.Responses
{
    public class OrderResponce
    {
        public required decimal TotalPrice { get; init; }

        public required string BestExecutionResponce { get; init; }
    }
}