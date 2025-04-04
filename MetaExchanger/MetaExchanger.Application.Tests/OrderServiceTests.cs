using FluentValidation;
using MetaExchanger.Application.Domain;
using MetaExchanger.Application.Infrastructure;
using MetaExchanger.Application.Services;
using Microsoft.EntityFrameworkCore;
using NSubstitute;

namespace MetaExchanger.Application.Tests
{
    [TestFixture]
    public class OrderServiceTests
    {
        private ApplicationDbContext _dbContext;
        private IOrderService _orderService;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            _dbContext =  new ApplicationDbContext(options);

            // add test data
            var crGuid = Guid.NewGuid();
            _dbContext.CryptoExchanges.Add(new Models.CryptoExchange() { Id = crGuid,  AcqTime = DateTime.UtcNow, BalanceBTC = 20, BalanceEUR = 20000});
            _dbContext.Orders.Add(new Models.Order() { Id = Guid.NewGuid(), Time = DateTime.UtcNow, CryptoExchangeId = crGuid, Type = "Buy", Amount = 1.5667m, Kind = "Limit", Price = 1.456m });
            _dbContext.Orders.Add(new Models.Order() { Id = Guid.NewGuid(), Time = DateTime.UtcNow, CryptoExchangeId = crGuid, Type = "Buy", Amount = 1.5667m, Kind = "Limit", Price = 1.856m });
            _dbContext.Orders.Add(new Models.Order() { Id = Guid.NewGuid(), Time = DateTime.UtcNow, CryptoExchangeId = crGuid, Type = "Sell", Amount = 6.5667m, Kind = "Limit", Price = 6.456m });
            _dbContext.SaveChanges();

            _orderService = new OrderService(_dbContext);            
        }

        [Test]
        public async Task Process_As_Selling()
        {
            // Arrange
            var domainOrder = new DomainOrder() { Amount = 1, Type = Common.OperationType.Sell, Id = Guid.NewGuid(), Kind = Common.Kind.Limit, Time = DateTime.Now };
            var expectedPrice = 1.856m;
            var ordersListResult = new List<DomainOrder>();

            // Act
            var result = await _orderService.ProcessAsSell(domainOrder, ordersListResult);

            // Assert
            Assert.That(result.Ok, Is.EqualTo(true));
            Assert.That(expectedPrice, Is.EqualTo(ordersListResult.First().Price!.Value));
        }

        [Test]
        public async Task Process_As_Buying()
        {
            // Arrange
            var expectedAmount = 2;
            var domainOrder = new DomainOrder() { Amount = expectedAmount, Type = Common.OperationType.Buy, Id = Guid.NewGuid(), Kind = Common.Kind.Limit, Time = DateTime.Now };
            var ordersListResult = new List<DomainOrder>();

            // Act
            var result = await _orderService.ProcessAsBuy(domainOrder, ordersListResult);

            // Assert
            Assert.That(result.Ok, Is.EqualTo(true));
            Assert.That(ordersListResult.Count(), Is.EqualTo(1));
            Assert.That(ordersListResult.First().Amount, Is.EqualTo(expectedAmount));
        }

        [TearDown]
        public void TearDown()
        {
            _dbContext?.Dispose(); // Dispose context
        }
    }
}