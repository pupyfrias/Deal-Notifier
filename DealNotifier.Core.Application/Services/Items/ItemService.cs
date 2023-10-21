using AutoMapper;
using DealNotifier.Core.Application.Exceptions;
using DealNotifier.Core.Application.Interfaces;
using DealNotifier.Core.Application.Interfaces.Repositories;
using DealNotifier.Core.Application.Interfaces.Services.Items;
using DealNotifier.Core.Application.Utilities;
using DealNotifier.Core.Application.ViewModels.V1.Item;
using DealNotifier.Core.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;
using System.Data;
using ILogger = Serilog.ILogger;
using OnlineStore = DealNotifier.Core.Application.Enums.OnlineStore;

namespace DealNotifier.Core.Application.Services.Items
{
    public class ItemService : GenericService<Item>, IItemService
    {
        private readonly IItemManagerService _itemManager;
        private readonly IItemRepository _itemRepository;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _memoryCache;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        #region Constructor

        public ItemService(
            IMapper mapper,
            IHttpContextAccessor httpContext,
            IMemoryCache memoryCache,
            IItemRepository itemRepository,
            ILogger logger,
            IItemManagerService itemManager,
            IServiceScopeFactory serviceScopeFactory

            ) :
            base(itemRepository, mapper, httpContext, memoryCache)
        {
            _itemRepository = itemRepository;
            _mapper = mapper;
            _memoryCache = memoryCache;
            _logger = logger;
            _itemManager = itemManager;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _itemRepository.GetByPublicIdAsync(id);

            if (entity is null)
            {
                throw new NotFoundException("Item", id);
            }

            await _itemRepository.DeleteAsync(entity);
            CacheUtility.InvalidateCache<Item>(_memoryCache);
        }

        public async Task<TDestination?> GetByPublicIdProjectedAsync<TDestination>(Guid id)
        {
            var mappedEntity = await _itemRepository.GetByPublicIdProjected<TDestination>(id);
            if (mappedEntity is null)
            {
                throw new NotFoundException("Item", id);
            }

            return mappedEntity;
        }

        #endregion Constructor

        public async Task SaveOrUpdateRangeAsync(ConcurrentBag<ItemCreateRequest> itemCreateList)
        {
            try
            {
                ConcurrentBag<Item> itemListToCreate = new();
                ConcurrentBag<Item> itemListToUpdate = new();

                await _itemManager.SplitExistingItemsFromNewItems(itemCreateList, itemListToCreate, itemListToUpdate);
                var createTask = CreateRangeAsync(itemListToCreate);
                var updateTask = UpdateRangeAsync(itemListToUpdate);
                await Task.WhenAll(updateTask, createTask);

                _itemManager.AddNewItemIdToEvaluatedIdList(itemListToCreate);
                _logger.Information($"{itemListToCreate.Count} items created | {itemListToUpdate.Count} items updated");
            }
            catch (Exception ex)
            {
                _logger.Error($"An error occurred while saving data to the items table. Exception: {ex.Message} InnerException: {ex.InnerException}", ex);
            }
        }

        public async Task UpdateAsync<TSource>(Guid id, TSource source) where TSource : IHasId<Guid>
        {
            if (!id!.Equals(source.Id))
            {
                throw new BadRequestException("The source ID does not match the provided ID");
            }

            var entity = await _itemRepository.GetByPublicIdAsync(id);

            if (entity == null)
            {
                throw new NotFoundException("Item", id);
            }

            _mapper.Map(source, entity);
            await _itemRepository.UpdateAsync(entity);
            CacheUtility.InvalidateCache<Item>(_memoryCache);
        }

        public async Task UpdateStockStatusAsync(OnlineStore onlineStore)
        {
            string query = "EXEC Update_StockStatus @IdListString, @OnlineStoreId, @OutputResult OUTPUT, @ErrorMessage OUTPUT";

            var idListString = string.Join(',', _itemManager.EvaluatedItemIdList);

            var idListStringParameter = new SqlParameter("@IdListString", idListString);
            var onlineStoreIdParameter = new SqlParameter("@OnlineStoreId", (int)onlineStore);
            var outputResultParameter = new SqlParameter("@OutputResult", SqlDbType.Bit) { Direction = ParameterDirection.Output };
            var errorMessageParameter = new SqlParameter("@ErrorMessage", SqlDbType.NVarChar, 4000) { Direction = ParameterDirection.Output };

            await _itemRepository.UpdateStockStatusAsync(query, idListStringParameter, onlineStoreIdParameter, outputResultParameter, errorMessageParameter);

            _logger.Information($"{_itemManager.EvaluatedItemIdList.Count} Items In Stock");
            _itemManager.EvaluatedItemIdList.Clear();
        }


        private async Task CreateRangeAsync(IEnumerable<Item> itemListToCreate)
        {
            using(var scope = _serviceScopeFactory.CreateScope()) 
            {
                var itemRepository = scope.ServiceProvider.GetRequiredService<IItemRepository>();
                await itemRepository.CreateRangeAsync(itemListToCreate);
            }
            
        }


        private async Task UpdateRangeAsync(IEnumerable<Item> itemListToUpdate)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var itemRepository = scope.ServiceProvider.GetRequiredService<IItemRepository>();
                await itemRepository.UpdateRangeAsync(itemListToUpdate);
            }
            
        }



    }
}