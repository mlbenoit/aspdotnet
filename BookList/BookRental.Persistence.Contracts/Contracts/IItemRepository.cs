using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookRental.Entities;

namespace BookRental.Persistence.Contracts
{
    public interface IItemRepository
    {
        Task<ItemEntity> CreateAsync(ItemEntity itemEntity);
        Task<ItemEntity> GetById(int itemId);
        Task<ItemEntity> UpdateAsync(ItemEntity itemEntity);
        Task<List<ItemEntity>> GetAllAsReadOnlyAsync(bool excludeDeleted = true);
    }
}
