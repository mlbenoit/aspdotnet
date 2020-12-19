using System;
using System.Threading.Tasks;
using BookRental.Entities;
using Microsoft.AspNetCore.Identity;


namespace BookRental.Services.Contracts.Identity
{
    public interface IUserManager
    {
        Task<UserEntity> FindByIdAsync(string userId);
        Task<UserEntity> FindByNameAsync(string userId);
        Task<UserEntity> FindByEmailAsync(string userId);
        Task<IdentityResult> CreateAsync(UserEntity userEntity, string password);
        Task<IdentityResult> UpdateAsync(UserEntity userEntity);
        Task<string> GetPasswordResetTokenAsync(UserEntity userEntity);
    }

}
        
