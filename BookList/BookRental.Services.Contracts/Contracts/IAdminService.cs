using System;
using System.Security.Claims;
using System.Threading.Tasks;
using BookRental.WebContracts;

namespace BookRental.Services.Contracts
{
    public interface IAdminService
    {
        Task<ServiceResult<CreateAdminResponse>> CreateAdminAsync(
                       CreateAdminRequest creteAdminRequest, ClaimsPrincipal authenticatedUser);
        Task<ServiceResult<bool>> LoginAsync(AdminLoginRequest request);
    }
}
