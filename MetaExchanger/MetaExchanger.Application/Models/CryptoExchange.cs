using System.ComponentModel.DataAnnotations;

namespace MetaExchanger.Application.Models
{
    /// <summary>
    /// DbContext entity, contains data about available  cryptoExchange and its offers.
    /// </summary>
    public class CryptoExchange
    {
        [Key]
        public required Guid Id { get; init; }

        public decimal BalanceEUR { get; set; }

        public decimal BalanceBTC { get; set; }

        /// <summary>
        /// Contains order list.
        /// </summary>
        public List<Order> Bids { get; set; } = [];

        public required DateTime AcqTime { get; init; }
    }
}
