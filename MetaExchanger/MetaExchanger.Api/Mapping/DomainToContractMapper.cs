using MetaExchanger.Application.Domain;
using MetaExchanger.Contracts.Responses;
using Newtonsoft.Json;
using System.Data;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;

namespace MetaExchanger.Api.Mapping
{
    public static class DomainToContractMapper
    {
        public static OrderResponce ToOrderResponce(this IEnumerable<DomainOrder> domainOrders)
        {
            var totalPrice = domainOrders.Select(o => o.Price * o.Amount).Sum();

            var options = new JsonSerializerOptions {
                NumberHandling =
                    JsonNumberHandling.AllowReadingFromString |
                    JsonNumberHandling.WriteAsString,
                WriteIndented = true
            };
            //string jsonString = JsonSerializer.Serialize(domainOrders);
            var j = JsonConvert.SerializeObject(domainOrders);
            Console.WriteLine($"\n{j}\n");
            return new OrderResponce() { BestExecutionResponce = j, TotalPrice = totalPrice ?? 0 };            
        }
    }
}