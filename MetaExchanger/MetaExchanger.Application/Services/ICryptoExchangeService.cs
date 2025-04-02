using MetaExchanger.Application.Common;
using MetaExchanger.Application.Domain;
using MetaExchanger.Application.Models;

namespace MetaExchanger.Application.Services
{
    public interface ICryptoExchangeService
    {
        Task<Result<IEnumerable<DomainOrder>>> CreateAsync(DomainOrder movie, CancellationToken token = default);
    }
}