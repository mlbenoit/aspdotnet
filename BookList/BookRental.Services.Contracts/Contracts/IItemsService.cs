using System;
using System.Security.Claims;
using System.Threading.Tasks;
using BookRental.WebContracts.WebContracts;

namespace BookRental.Services.Contracts
{
    public interface IItemsService
    {
        Task<ServiceResult<CreateItemResponse>> CreateItemAsync(CreateItemRequest request, ClaimsPrincipal authenticatedUser);
        Task<ServiceResult<UpdateItemResponse>> UpdateItemAsync(UpdateItemRequest request, ClaimsPrincipal authenticatedUser);
    }
}
