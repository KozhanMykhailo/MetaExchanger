﻿using System.ComponentModel.DataAnnotations;

namespace MetaExchanger.Application.Models
{
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
