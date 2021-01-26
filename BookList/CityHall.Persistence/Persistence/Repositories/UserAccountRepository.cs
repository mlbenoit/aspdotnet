using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookRental.Entities;
using BookRental.Persistence.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace BookRental.Persistence
{
    public class UserAccountRepository:IUserAccountRepository
    {
        private readonly BookDbContext _context;

        public UserAccountRepository(BookDbContext context)
        {
            _context = context;
        }

        public async Task<List<UserEntity>> GetAllAsReadOnlyAsync()
        {
            return await _context.Users
                                  .AsNoTracking()
                                  .ToListAsync();
        }

        public async Task<UserEntity> GetByIdAsReadOnlyAsync(string userId)
        {
            return await _context.Users
                                 .AsNoTracking()
                                 .SingleOrDefaultAsync(user => user.Id == userId);
        }

        public async Task<UserEntity> GetByUserNameAsyc(string userName)
        {
            return await _context.Users
                                  .AsNoTracking()
                                  .SingleOrDefaultAsync(user => user.UserName == userName);
        }

        public async Task<UserEntity> CreateAsync(UserEntity userEntity)
        {
            //EntityEntry class provides acces to change tracking information and
            //operations for a given entity
            //DbContext.AdAsync begins tracking the given entity, and any other reachable
            //entities that are not already tracked in the Added state such that they wil be
            //inserted into the database when SaveChanges() is called. 
            EntityEntry<UserEntity> entityEntry = await _context.Users.AddAsync(userEntity);
            await _context.SaveChangesAsync();
            return entityEntry.Entity; // gets the entity tracked by this entry
        }
    }
}
