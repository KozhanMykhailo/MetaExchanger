using MetaExchanger.Application.Common;
using MetaExchanger.Application.Domain;

namespace MetaExchanger.Application.Services
{
    /// <summary>
    /// Interface with abstract definitions of crypto exchange service.
    /// </summary>
    public interface ICryptoExchangeService
    {
        /// <summary>
        /// Process GET request. Find the best execution for request as list of Orders.
        /// </summary>
        Task<Result<IEnumerable<DomainOrder>>> GetBestExecutionAsync(DomainOrder movie, CancellationToken token = default);
    }
}