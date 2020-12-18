using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace BookList.Services.Contracts.Identity
{
    public interface ISignInManager
    {
        Task<SignInResult> PasswordSignInAsync(string userName, string password,
                                          bool isPersistent, bool lockoutOnFailure);
    }
}
