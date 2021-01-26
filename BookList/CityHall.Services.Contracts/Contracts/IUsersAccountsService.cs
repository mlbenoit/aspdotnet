using System;
using System.Security.Claims;
using System.Threading.Tasks;
using BookRental.WebContracts;
using BookRental.WebContracts.WebContracts;

namespace BookRental.Services.Contracts
{
    public interface IUsersAccountsService
    {
        Task<ServiceResult<CreateUserResponse>>CreateUserResponseAsync(CreateUserRequest request, ClaimsPrincipal authenticatedUser);
        //Task<ServiceResult<UpdateUserResponse>> UpdateUserResponseAsync(UpdateUserRequest request, ClaimsPrincipal authenticatedUser);
        Task<ServiceResult<bool>> LoginAsync(UserLoginRequest request);

    }
}
