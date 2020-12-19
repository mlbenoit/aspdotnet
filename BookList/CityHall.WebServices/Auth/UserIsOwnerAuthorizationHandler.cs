using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using BookList.Entities;
using BookList.Services.Contracts.Contracts.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;

namespace BookList.WebServices.Auth
{
    public class UserIsOwnerAuthorizationHandler
        :AuthorizationHandler<OperationAuthorizationRequirement, UserEntity>
    {
        private readonly UserManager<UserEntity> _userManager;

        public UserIsOwnerAuthorizationHandler(UserManager<UserEntity> userManager)
        {
            _userManager = userManager;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            OperationAuthorizationRequirement requirement, UserEntity resource)
        {
            if (context.User == null || resource == null)
            {
                //Return Task.FromResult(0)if targeting a vesion
                // >Net Framework older than 4.6
                return Task.CompletedTask;
            }

            if (requirement == null)
            {
                return Task.CompletedTask;
            }

            if(requirement.Name != Operations.Create.Name &&
               requirement.Name != Operations.Read.Name &&
               requirement.Name != Operations.Update.Name &&
               requirement.Name != Operations.Delete.Name)
            {
                return Task.CompletedTask;
            }

            string authenticatedUserName = context.User.FindFirst(JwtRegisteredClaimNames.Sub).Value;
            if(string.Equals(resource.UserName, authenticatedUserName, StringComparison.InvariantCultureIgnoreCase))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }

    }
}
