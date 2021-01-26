using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using BookRental.Entities;
using BookRental.Persistence.Contracts;
using BookRental.Services.Contracts;
using BookRental.Services.Contracts.Identity;
using BookRental.WebContracts;
using BookRental.WebContracts.WebContracts;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace BookRental.Services
{
    public class UsersAccountsService : IUsersAccountsService
    {
        private readonly IUserAccountRepository _userAccountRepository;
        private readonly ISignInManager _signInManager;
        private readonly IUserManager _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        

        public UsersAccountsService(IUserAccountRepository userAccountRepository,
                                    ISignInManager signInManager,
                                    IUserManager userManager,
                                    IHttpContextAccessor httpContextAccessor)
        {
            _userAccountRepository = userAccountRepository;
            _signInManager = signInManager;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ServiceResult<CreateUserResponse>> CreateUserResponseAsync(CreateUserRequest request, ClaimsPrincipal authenticatedUser)
        {
            ServiceResult<CreateUserResponse> errorResult =
                                new ServiceResult<CreateUserResponse>(HttpStatusCode.BadRequest);
            if(request == null
                || string.IsNullOrEmpty(request.Password)
                || string.IsNullOrEmpty(request.Email)
                || string.IsNullOrEmpty(request.FirstName)
                || string.IsNullOrEmpty(request.LastName)
                || string.IsNullOrEmpty(request.MiddleName)
                || string.IsNullOrEmpty(request.Address)
                || string.IsNullOrEmpty(request.City)
                || string.IsNullOrEmpty(request.ZipCode)
                || (request.DateOfBirth == null)
                || string.IsNullOrEmpty(request.Gender))
            {
                return errorResult;

            }

            DateTime utcNow = DateTime.UtcNow;

            UserEntity userEntity = new UserEntity
            {
                
                Password = request.Password,
                Email = request.Email,
                UserName = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                MiddleName = request.MiddleName,
                Address = request.Address,
                City = request.City,
                ZipCode = request.ZipCode,
                Gender = request.Gender,
                IsDeleted = false,
                CreatedOn = utcNow,
            };


            await _userManager.CreateAsync(userEntity, request.Password);

            return new ServiceResult<CreateUserResponse>(new CreateUserResponse());

        }

        public async Task<ServiceResult<bool>> LoginAsync(UserLoginRequest request)
        {
            if(request == null
                || string.IsNullOrEmpty(request.Email)
                || string.IsNullOrEmpty(request.Password))
            {
                return new ServiceResult<bool>(HttpStatusCode.BadRequest);
            }

            //SignInManager provides the APIs for user sign in.
            SignInResult result = await _signInManager.PasswordSignInAsync(
                                        request.Email, request.Password,
                                        isPersistent: false,
                                        lockoutOnFailure: false);

            if (!result.Succeeded)
            {
                return new ServiceResult<bool>(HttpStatusCode.Unauthorized);
            }

            UserEntity userEntity = await _userManager.FindByEmailAsync(request.Email);
            if (userEntity == null)
            {
                // This situation may arise when the user is deleted in another thread
                return new ServiceResult<bool>(HttpStatusCode.Unauthorized);
            }

            List<Claim> claims = new List<Claim>
             {
                 new Claim(ClaimTypes.Email, request.Email),
                 new Claim("FullName", $"{userEntity.FirstName} {userEntity.LastName}"),
                 new Claim(ClaimTypes.Role, "Users"),
             };
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            AuthenticationProperties authProperties = new AuthenticationProperties
            {
                //AllowRefresh = <bool>,
                // Refreshing the authentication session should be allowed.

                //ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                // The time at which the authentication ticket expires. A
                // value set here overrides the ExpireTimeSpan option of
                // CookieAuthenticationOptions set with AddCookie.

                //IsPersistent = true,
                // Whether the authentication session is persisted across
                // multiple requests. When used with cookies, controls
                // whether the cookie's lifetime is absolute (matching the
                // lifetime of the authentication ticket) or session-based.

                //IssuedUtc = <DateTimeOffset>,
                // The time at which the authentication ticket was issued.

                //RedirectUri = <string>
                // The full path or absolute URI to be used as an http
                // redirect response value.
            };

            await _httpContextAccessor.HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            //_logger.LogInformation("User {Email} logged in at {Time}.", user.Email, DateTime.UtcNow);
            return new ServiceResult<bool>(true);

        }

        //public Task<ServiceResult<UpdateUserResponse>> UpdateUserResponseAsync(UpdateUserRequest updateUserResponse, ClaimsPrincipal authenticatedUser)
        //{
        //   // throw new NotImplementedException();
        //}


    }
}
