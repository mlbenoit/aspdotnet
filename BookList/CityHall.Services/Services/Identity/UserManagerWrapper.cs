using System;
using System.Threading.Tasks;
using BookRental.Entities;
using BookRental.Services.Contracts.Identity;
using Microsoft.AspNetCore.Identity;

namespace Bookrental.Services.Identity
{
    public class UserManagerWrapper: IUserManager
    {
        private readonly UserManager<UserEntity> _userManager;

        public UserManagerWrapper(UserManager<UserEntity> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IdentityResult> CreateAsync(UserEntity userEntity, string password)
        {
            return await _userManager.CreateAsync(userEntity, password);
        }

        public async Task<UserEntity> FindByEmailAsync(string emailId)
        {
            return await _userManager.FindByEmailAsync(emailId);
        }

        public async Task<UserEntity> FindByIdAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }

        public async Task<UserEntity> FindByNameAsync(string userName)
        {
            return await _userManager.FindByNameAsync(userName);
        }

        public async Task<string> GetPasswordResetTokenAsync(UserEntity userEntity)
        {
            return await _userManager.GeneratePasswordResetTokenAsync(userEntity);
        }

        public async Task<IdentityResult> UpdateAsync(UserEntity userEntity)
        {
            return await _userManager.UpdateAsync(userEntity);
        }
    }
}
