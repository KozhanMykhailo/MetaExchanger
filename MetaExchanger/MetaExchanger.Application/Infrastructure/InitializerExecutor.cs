using MetaExchanger.Application.Infrastructure.HelpersModels;
using MetaExchanger.Application.Models;
using Newtonsoft.Json;

namespace MetaExchanger.Application.Infrastructure
{
    /// <summary>
    /// Contains functionality for initialization data from .txt file.
    /// </summary>
    public static class InitializerExecutor
    {
        /// <summary>
        /// Read and deserialize from file.
        /// </summary>
        /// <param name="fileName">from solution</param>
        /// <returns>List of CryptoExchanges</returns>
        public static List<CryptoExchange> CreateEntitiesFromFile(string fileName)
        {
            var cryptoExchanges = new List<CryptoExchange>();

            string filePath = Path.Combine(Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory), fileName);

            var count = 3;// needs only 3 row from file
            using (var sr = new StreamReader(filePath))
            {
                for (int i = 1; i <= count; i++)
                {
                    var line = sr.ReadLine();
                    var splitResult = line!.Split('\t');
                    var jsonstring = splitResult[1];
                    Root root = JsonConvert.DeserializeObject<Root>(jsonstring);

                    var cryptoExchange = CreateContextEntities(root);
                    cryptoExchanges.Add(cryptoExchange);
                }
            }

            return cryptoExchanges;
        }

        /// <summary>
        /// Create and fill a CryptoExchange with Bids.
        /// </summary>
        /// <param name="root"></param>
        /// <returns>CryptoExchange entity</returns>
        private static CryptoExchange CreateContextEntities(Root? root)
        {
            var guid = Guid.NewGuid();
            var newCryptoExchange = new CryptoExchange()
            {
                Id = guid,
                AcqTime = root.AcqTime,
                BalanceBTC = 10m,
                BalanceEUR = 100000m
            };

            foreach (var bid in root.Bids)
            {
                newCryptoExchange.Bids.Add(new Models.Order()
                {
                    Id = Guid.NewGuid(),
                    CryptoExchangeId = newCryptoExchange.Id,
                    Amount = bid.Order.Amount,
                    Kind = "Limit",
                    Type = bid.Order.Type,
                    Time = bid.Order.Time,
                    Price = bid.Order.Price
                });
            }

            return newCryptoExchange;
        }
    }
}