using System;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using BookRental.Entities;
using BookRental.Persistence.Contracts;
using BookRental.Services.Contracts;
using BookRental.WebContracts.WebContracts;

namespace BookRental.Services
{
    public class ItemsService: IItemsService
    {
        private readonly IItemRepository _itemRepository;
        public ItemsService(IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }

        public async Task<ServiceResult<CreateItemResponse>> CreateItemAsync(CreateItemRequest request, ClaimsPrincipal authenticatedUser)
        {
            ServiceResult<CreateItemResponse> errorResult = new ServiceResult<CreateItemResponse>(HttpStatusCode.BadRequest);

            if (request == null)
            {
                return errorResult;
            }


            DateTime utcNow = DateTime.UtcNow;

            ItemEntity itemEntity = new ItemEntity
            {
                // Add auto Generated ID
                Name = request.Name,
                Price = request.Price,
                CreatedOn = utcNow,
                IsDeleted = false,

            };

            await _itemRepository.CreateAsync(itemEntity);

            return new ServiceResult<CreateItemResponse>(new CreateItemResponse());
        }

        public async Task<ServiceResult<UpdateItemResponse>> UpdateItemAsync(UpdateItemRequest request, ClaimsPrincipal authenticatedUser)
        {
            ServiceResult<UpdateItemResponse> errorResult = new ServiceResult<UpdateItemResponse>(HttpStatusCode.BadRequest);

            if(request == null)
            {
                return errorResult;
            }

            ItemEntity itemEntity = await _itemRepository.GetById(request.Id);
            DateTime utcNow = DateTime.UtcNow;

            itemEntity.Name = request.Name;
            itemEntity.Price = request.Price;
            itemEntity.UpdatedOn = utcNow;

            await _itemRepository.UpdateAsync(itemEntity);

            return new ServiceResult<UpdateItemResponse>(new UpdateItemResponse());

            
        }
    }
}
