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
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace BookRental.Services
{
    public class AdminService:IAdminService
    {
        private readonly IUserManager _userManager;
        private readonly ISignInManager _signInManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserAccountRepository _userAccountRepository;

        public AdminService(IUserManager userManager,
            ISignInManager signInManager,
            IHttpContextAccessor httpContextAccessor,
            IUserAccountRepository userAccountRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _httpContextAccessor = httpContextAccessor;
            _userAccountRepository = userAccountRepository;

        }

        public Task<ServiceResult<CreateAdminResponse>> CreateAdminAsync(
            CreateAdminRequest createAdminRequest, ClaimsPrincipal claimsPrincipal)
        {
            return null;
        }


        public async Task<ServiceResult<bool>> LoginAsync(AdminLoginRequest request)
        {
            if(request == null
                || string.IsNullOrWhiteSpace(request.Email)
                || string.IsNullOrWhiteSpace(request.Password))
            {
                return new ServiceResult<bool>(HttpStatusCode.BadRequest);
            }

            SignInResult result = await _signInManager.PasswordSignInAsync(request.Email,
                request.Password, isPersistent: false, lockoutOnFailure: false);

            if (!result.Succeeded)
            {
                return new ServiceResult<bool>(HttpStatusCode.Unauthorized);
            }

            UserEntity userEntity = await _userManager.FindByEmailAsync(request.Email);
            if(userEntity == null)
            {
                // This situation may arise when the  user is deleted in another thread
                return new ServiceResult<bool>(HttpStatusCode.Unauthorized);
            }

            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, request.Email),
                new Claim("FullName", $"{userEntity.FirstName} {userEntity.LastName}"),
                new Claim(ClaimTypes.Role, "Administrator"),
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

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

            //_logger.LogInformation("user {Email} logged in at {Time}." user.Email, DateTime.UtcNow);
            return new ServiceResult<bool>(true);
        }




    }
}
