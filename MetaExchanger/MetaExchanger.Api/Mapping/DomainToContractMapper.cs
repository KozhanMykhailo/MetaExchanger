using MetaExchanger.Application.Domain;
using MetaExchanger.Contracts.Responses;
using System.Data;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MetaExchanger.Api.Mapping
{
    /// <summary>
    /// Map IEnumerable<DomainOrder> to OrderResponce.
    /// </summary>
    public static class DomainToContractMapper
    {
        public static OrderResponce ToOrderResponce(this IEnumerable<DomainOrder> domainOrders)
        {
            var totalPrice = domainOrders.Select(o => o.Price * o.Amount).Sum();

            var options = new JsonSerializerOptions {
                NumberHandling =
                    JsonNumberHandling.AllowReadingFromString |
                    JsonNumberHandling.WriteAsString,
                WriteIndented = false
            };
            string jsonString = JsonSerializer.Serialize(domainOrders, options);
            Console.WriteLine($"\n{jsonString}\n");
            return new OrderResponce() { BestExecutionResponce = jsonString, TotalPrice = totalPrice ?? 0 };            
        }
    }
}