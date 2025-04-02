using System.Text.Json.Serialization;

namespace MetaExchanger.Application.Common
{
    [JsonConverter(typeof(JsonStringEnumConverter<OperationType>))]
    public enum OperationType
    {
        Buy,
        Sell
    }

    public enum Kind
    {
        Limit
    }
}