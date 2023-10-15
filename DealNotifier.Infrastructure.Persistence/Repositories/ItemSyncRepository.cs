using AutoMapper;
using AutoMapper.QueryableExtensions;
using DealNotifier.Core.Application.Interfaces.Repositories;
using DealNotifier.Core.Domain.Entities;
using DealNotifier.Infrastructure.Persistence.DbContexts;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace DealNotifier.Infrastructure.Persistence.Repositories
{
    public class ItemSyncRepository :  IItemSyncRepository
    {
        private readonly ApplicationDbContext _dbContext;


        #region Constructor

        public ItemSyncRepository(ApplicationDbContext context)
        {
            _dbContext = context;
        }

        public async Task UpdateStockStatusAsync(string query, SqlParameter idListString, SqlParameter onlineStoreId, SqlParameter outputResult, SqlParameter errorMessage)
        {
            await _dbContext.Database.ExecuteSqlRawAsync(query, idListString, onlineStoreId, outputResult, errorMessage);
            var successful = (bool)outputResult.Value;

            if (!successful) throw new Exception(errorMessage.Value.ToString());

        }




        #endregion Constructor
    }
}