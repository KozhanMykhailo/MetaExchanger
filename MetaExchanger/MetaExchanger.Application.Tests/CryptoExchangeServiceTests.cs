using FluentValidation;
using MetaExchanger.Application.Domain;
using MetaExchanger.Application.Infrastructure;
using MetaExchanger.Application.Services;
using Microsoft.EntityFrameworkCore;
using NSubstitute;

namespace MetaExchanger.Application.Tests
{
    [TestFixture]
    public class CryptoExchangeServiceTests
    {
        private ApplicationDbContext _dbContext;
        private IValidator<DomainOrder> _orderValidator;
        private ICryptoExchangeService _cryptoExchangeService;

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


            _orderValidator = Substitute.For<IValidator<DomainOrder>>();
            _orderValidator.Validate(Arg.Any<DomainOrder>()).Returns(new FluentValidation.Results.ValidationResult());

            _cryptoExchangeService = new CryptoExchangeService(_orderValidator, _dbContext);
        }

        [Test]
        public async Task Get_Highest_Price_For_Selling()
        {
            // Arrange
            var domainOrder = new DomainOrder() { Amount = 1, Type = Common.OperationType.Sell, Id = Guid.NewGuid(), Kind = Common.Kind.Limit, Time = DateTime.Now };
            var expectedPrice = 1.856m;

            // Act
            var result = await _cryptoExchangeService.GetOrdersAsync(domainOrder);

            // Assert
            Assert.That(result.Ok, Is.EqualTo(true));
            Assert.That(expectedPrice, Is.EqualTo(result.Value!.First().Price!.Value));
        }

        [Test]
        public async Task Get_Best_Execution_For_Buying()
        {
            // Arrange
            var expectedAmount = 2;
            var domainOrder = new DomainOrder() { Amount = expectedAmount, Type = Common.OperationType.Buy, Id = Guid.NewGuid(), Kind = Common.Kind.Limit, Time = DateTime.Now };

            // Act
            var result = await _cryptoExchangeService.GetOrdersAsync(domainOrder);

            // Assert
            Assert.That(result.Ok, Is.EqualTo(true));
            Assert.That(result.Value!.Count(), Is.EqualTo(1));
            Assert.That(result.Value!.First().Amount, Is.EqualTo(expectedAmount));
        }

        [Test]
        public async Task Get_Best_Execution_For_Buying_Failed()
        {
            // Arrange
            var expectedAmount = 25;
            var domainOrder = new DomainOrder() { Amount = expectedAmount, Type = Common.OperationType.Buy, Id = Guid.NewGuid(), Kind = Common.Kind.Limit, Time = DateTime.Now };

            // Act
            var result = await _cryptoExchangeService.GetOrdersAsync(domainOrder);

            // Assert
            Assert.That(result.Ok, Is.EqualTo(false));
            Assert.IsTrue(result.Error!.Message.StartsWith("CryptoExchanges don't have enough BTC for dealing, not enough:"));
        }

        [TearDown]
        public void TearDown()
        {
            _dbContext?.Dispose(); // Dispose context
        }
    }
}