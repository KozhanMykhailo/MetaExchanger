using MetaExchanger.Application.Common;
using MetaExchanger.Application.Domain;

namespace MetaExchanger.Application.Services
{
    /// <summary>
    /// Interface with abstract definitions of crypto exchange service.
    /// </summary>
    public interface ICryptoExchangeService
    {
        Task<Result<IEnumerable<DomainOrder>>> GetBestExecutionAsync(DomainOrder movie, CancellationToken token = default);
    }
}