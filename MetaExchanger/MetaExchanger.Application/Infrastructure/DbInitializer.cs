using MetaExchanger.Application.Common;
using MetaExchanger.Application.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace MetaExchanger.Application.Infrastructure
{
    public class DbInitializer
    {
        private readonly ApplicationDbContext _context;

        public DbInitializer(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task InitializeAsync()
        {
            if (await _context.CryptoExchanges.AnyAsync())
                return;

            var result = CreateEntitiesFromFile(@"Infrastructure/order_books_data.txt");

            foreach (var cryptoExchange in result) 
            {
                await _context.CryptoExchanges.AddAsync(cryptoExchange);

                foreach (var order in cryptoExchange.Bids)
                {
                    await _context.Orders.AddAsync(order);
                }
            }            

            await _context.SaveChangesAsync();
        }

        private List<CryptoExchange> CreateEntitiesFromFile(string fileName)
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

        private CryptoExchange CreateContextEntities(Root? root)
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

        public class Bid
        {
            public Order Order { get; set; }
        }

        public class Order
        {
            public object Id { get; set; }
            public DateTime Time { get; set; }
            public string Type { get; set; }
            public string Kind { get; set; }
            public decimal Amount { get; set; }
            public decimal Price { get; set; }
        }

        public class Root
        {
            public DateTime AcqTime { get; set; }
            public List<Bid> Bids { get; set; }
        }        
    }
}