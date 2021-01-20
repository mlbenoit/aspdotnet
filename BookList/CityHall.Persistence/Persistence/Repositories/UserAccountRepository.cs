using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookRental.Entities;
using BookRental.Persistence.Contracts;
using Microsoft.EntityFrameworkCore;

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
    }
}
