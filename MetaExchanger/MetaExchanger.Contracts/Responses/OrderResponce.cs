namespace MetaExchanger.Contracts.Responses
{
    public class OrderResponce
    {
        /// <summary>
        /// Total price of request(for buying or selling).
        /// </summary>
        public required decimal TotalPrice { get; init; }

        /// <summary>
        /// Json array with data.
        /// </summary>
        public required string BestExecutionResponce { get; init; }
    }
}