using MetaExchanger.Application.Common;
using MetaExchanger.Application.Domain;

namespace MetaExchanger.Application.Services
{
    /// <summary>
    /// Interface with abstract definitions of domain order service.
    /// </summary>
    public interface IOrderService
    {
        Task<Result> ProcessAsBuy(DomainOrder domainOrder, List<DomainOrder> responseResult);
        Task<Result> ProcessAsSell(DomainOrder domainOrder, List<DomainOrder> responseResult);
    }
}