using MetaExchanger.Application.Common;
using MetaExchanger.Application.Domain;

namespace MetaExchanger.Application.Services
{
    public interface ICryptoExchangeService
    {
        Task<Result<IEnumerable<DomainOrder>>> GetOrdersAsync(DomainOrder movie, CancellationToken token = default);
    }
}