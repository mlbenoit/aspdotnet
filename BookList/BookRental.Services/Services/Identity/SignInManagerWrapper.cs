using System;
using System.Threading.Tasks;
using BookRental.Entities;
using BookRental.Services.Contracts.Identity;
using Microsoft.AspNetCore.Identity;

namespace BookRental.Services.Identity
{
    public class SignInManagerWrapper : ISignInManager
    {
        private readonly SignInManager<UserEntity> _signInManager;

        public SignInManagerWrapper(SignInManager<UserEntity> signInManager)
        {
            _signInManager = signInManager;
        }

        public async Task<SignInResult> PasswordSignInAsync(string userName, string password,
                                                bool isPersistent, bool lookoutOnFailure)
        {
            return await _signInManager.PasswordSignInAsync(userName, password, isPersistent,
                                                                lookoutOnFailure);
        }
    }
}
