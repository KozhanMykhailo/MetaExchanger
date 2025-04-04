using System.Text.Json.Serialization;

namespace MetaExchanger.Application.Common
{
    /// <summary>
    /// Operation type.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter<OperationType>))]
    public enum OperationType
    {
        Buy,
        Sell
    }

    /// <summary>
    /// Kind. Only Limit now.
    /// </summary>
    public enum Kind
    {
        Limit
    }
}