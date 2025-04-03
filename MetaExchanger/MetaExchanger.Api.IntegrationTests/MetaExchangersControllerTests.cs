using FluentAssertions;
using MetaExchanger.Application.Domain;
using MetaExchanger.Contracts.Responses;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace MetaExchanger.Api.IntegrationTests
{
    [TestFixture]
    public class MetaExchangersControllerTests
    {
        [Test]
        public async Task Get_Best_Execution_For_Buy()
        {
            // Arrange
            var application = new WebApplicationFactory();
            var client = application.CreateClient();

            // Act
            var response = await client.GetAsync("/api/v1/metaexchangers?OperationType=Buy&Amount=10");

            // Assert
            response.EnsureSuccessStatusCode();

            var responceObj = await response.Content.ReadFromJsonAsync<OrderResponce>();
            var responceDomains = JsonConvert.DeserializeObject<IEnumerable<DomainOrder>>(responceObj!.BestExecutionResponce);
            responceObj?.TotalPrice.Should().Be(29534.609m);
            responceDomains!.Count().Should().Be(5);
        }

        [Test]
        public async Task Get_Best_Execution_For_Sell()
        {
            // Arrange
            var application = new WebApplicationFactory();
            var client = application.CreateClient();

            // Act
            var response = await client.GetAsync("/api/v1/metaexchangers?OperationType=Sell&Amount=2");

            // Assert
            response.EnsureSuccessStatusCode();

            var responceObj = await response.Content.ReadFromJsonAsync<OrderResponce>();
            var responceDomains = JsonConvert.DeserializeObject<IEnumerable<DomainOrder>>(responceObj!.BestExecutionResponce);
            responceObj?.TotalPrice.Should().Be(5921.34m);
            responceDomains!.Count().Should().Be(1);
        }
    }
}