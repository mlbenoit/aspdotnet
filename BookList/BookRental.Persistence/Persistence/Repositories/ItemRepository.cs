using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookRental.Entities;
using BookRental.Persistence.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace BookRental.Persistence.Persistence
{
    public class ItemRepository : IItemRepository
    {
        private readonly BookDbContext _context;

        public ItemRepository(BookDbContext context)
        {
            _context = context;
        }

        public async Task<ItemEntity> CreateAsync(ItemEntity itemEntity)
        {
            EntityEntry<ItemEntity> entityEntry = await _context.Items.AddAsync(itemEntity);
            await _context.SaveChangesAsync();
            return entityEntry.Entity;
        }

        public async Task<List<ItemEntity>> GetAllAsReadOnlyAsync(bool excludeDeleted = true)
        {
            return await _context.Items
                    .Where(e => !excludeDeleted || !e.IsDeleted)
                    .AsNoTracking()
                    .ToListAsync();
        }

        public async Task<ItemEntity> GetById(int itemId)
        {
            return await _context.Items
                    .FirstOrDefaultAsync(x => x.Id == itemId);
        }

        public async Task<ItemEntity> UpdateAsync(ItemEntity itemEntity)
        {
            itemEntity.UpdatedOn = DateTime.UtcNow;
            _context.Items.Update(itemEntity);
            await _context.SaveChangesAsync();
            return itemEntity;
        }
    }
}
