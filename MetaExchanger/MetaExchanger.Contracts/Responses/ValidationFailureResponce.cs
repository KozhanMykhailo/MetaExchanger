namespace MetaExchanger.Contracts.Responses
{
    /// <summary>
    /// Contains validation errors.
    /// </summary>
    public class ValidationFailureResponce
    {
        public required IEnumerable<ValidationResponce> Errors { get; init; }
    }

    /// <summary>
    /// Contains validation error of 1 property.
    /// </summary>
    public class ValidationResponce
    {
        /// <summary>
        /// Name of object property.
        /// </summary>
        public required string PropertyName { get; init; }
        /// <summary>
        /// Validation message.
        /// </summary>
        public required string Message { get; init; }
    }
}