﻿using FluentValidation;
using MetaExchanger.Application.Common;
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
        private ICryptoExchangeService _cryptoExchangeService;
        private IOrderService _orderService;
        private IValidator<DomainOrder> _orderValidator;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            _dbContext = new ApplicationDbContext(options);

            // add test data
            var crGuid = Guid.NewGuid();
            _dbContext.CryptoExchanges.Add(new Models.CryptoExchange() { Id = crGuid, AcqTime = DateTime.UtcNow, BalanceBTC = 20, BalanceEUR = 20000 });
            _dbContext.Orders.Add(new Models.Order() { Id = Guid.NewGuid(), Time = DateTime.UtcNow, CryptoExchangeId = crGuid, Type = "Buy", Amount = 1.5667m, Kind = "Limit", Price = 1.456m });
            _dbContext.Orders.Add(new Models.Order() { Id = Guid.NewGuid(), Time = DateTime.UtcNow, CryptoExchangeId = crGuid, Type = "Buy", Amount = 1.5667m, Kind = "Limit", Price = 1.856m });
            _dbContext.Orders.Add(new Models.Order() { Id = Guid.NewGuid(), Time = DateTime.UtcNow, CryptoExchangeId = crGuid, Type = "Sell", Amount = 6.5667m, Kind = "Limit", Price = 6.456m });
            _dbContext.SaveChanges();

            _orderService = Substitute.For<IOrderService>();
            _orderService.ProcessAsBuy(Arg.Any<DomainOrder>(), Arg.Any<List<DomainOrder>>()).Returns(new Error("CryptoExchanges don't have enough BTC for dealing, not enough:"));
            _orderService.ProcessAsSell(Arg.Any<DomainOrder>(), Arg.Any<List<DomainOrder>>()).Returns(Task.FromResult(Result.Empty));

            _orderValidator = Substitute.For<IValidator<DomainOrder>>();
            _orderValidator.Validate(Arg.Any<DomainOrder>()).Returns(new FluentValidation.Results.ValidationResult());

            _cryptoExchangeService = new CryptoExchangeService(_orderValidator, _dbContext, _orderService);
        }

        [Test]
        public async Task Get_Best_Execution_For_Buying_Failed()
        {
            // Arrange
            var expectedAmount = 25;
            var domainOrder = new DomainOrder() { Amount = expectedAmount, Type = Common.OperationType.Buy, Id = Guid.NewGuid(), Kind = Common.Kind.Limit, Time = DateTime.Now };

            // Act
            var result = await _cryptoExchangeService.GetBestExecutionAsync(domainOrder);

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