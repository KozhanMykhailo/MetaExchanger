namespace MetaExchanger.Contracts.Responses
{
    public class ValidationFailureResponce
    {
        public required IEnumerable<ValidationResponce> Errors { get; init; }
    }

    public class ValidationResponce
    {
        public required string PropertyName { get; init; }
        public required string Message { get; init; }
    }
}