﻿using MetaExchanger.Application.Infrastructure.HelpersModels;
using MetaExchanger.Application.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace MetaExchanger.Application.Infrastructure
{
    public class DbInitializer
    {
        private readonly ApplicationDbContext _context;

        public DbInitializer(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task InitializeAsync()
        {
            if (await _context.CryptoExchanges.AnyAsync())
                return;

            var result = InitializerExecutor.CreateEntitiesFromFile(@"Infrastructure/order_books_data.txt");

            foreach (var cryptoExchange in result) 
            {
                await _context.CryptoExchanges.AddAsync(cryptoExchange);

                foreach (var order in cryptoExchange.Bids)
                {
                    await _context.Orders.AddAsync(order);
                }
            }            

            await _context.SaveChangesAsync();
        }
    }
}