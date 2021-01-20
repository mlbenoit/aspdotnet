using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookRental.Entities;


namespace BookRental.Persistence.Contracts
{
    public interface IUserAccountRepository
    {
        Task<UserEntity> GetByUserNameAsyc(string userName);
        Task<List<UserEntity>> GetAllAsReadOnlyAsync();
        Task<UserEntity> GetByIdAsReadOnlyAsync(string userId);
    }
}
