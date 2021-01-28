using System;
using System.Threading.Tasks;
using BookRental.Persistence.Contracts;
using BookRental.Services.Contracts;
using BookRental.WebContracts;
using BookRental.WebContracts.WebContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookRental.WebServices.ControllersMvc
{
    [Route("user/Users")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class UsersMvcController:MvcControllerBase
    {
        private readonly IUserAccountRepository _userAccountRepository;
        private readonly IUsersAccountsService _usersAccountsService;


        public UsersMvcController(IUserAccountRepository userAccountRepository,
                                    IUsersAccountsService usersAccountsService)
        {
            _userAccountRepository = userAccountRepository;
            _usersAccountsService = usersAccountsService;
        }

        [AllowAnonymous]
        [HttpGet("Login", Name ="Users-Login")]
        public IActionResult Login()
        {
            return View("UserLogin");
        }

        [AllowAnonymous]
        [HttpPost("Login", Name ="Users-Login")]
        public async Task<IActionResult> Login(UserLoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ServiceResult<bool> result = await _usersAccountsService.LoginAsync(request);
            if (result.IsError)
            {
                ModelState.AddModelError("", "Invalid admin email or password");
                return View("UserLogin");
            }

            return RedirectToRoute("Users-Account");
        }

        [AllowAnonymous]
        [HttpGet("Add", Name = "Users-Add")]
        public IActionResult AddUserAsync()
        {
            return View("AddUser");
        }

        [AllowAnonymous]  // allow unauthrized users
        [HttpPost("Add", Name = "Users-Add")]
        public async Task<IActionResult> CreateUserAsync(CreateUserRequest request)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Some of your inputs are invalid");
                return View("AddUser");
            }

            ServiceResult<CreateUserResponse> result = await _usersAccountsService
                                                .CreateUserResponseAsync(request, this.User);
            if (result.IsError)
            {
                foreach (IServiceError item in result.ErrorMessages)
                {
                    ModelState.AddModelError("", item.Description);
                }

                ModelState.AddModelError("",
                             "Error creating book. Check your inputs before trying again");
                return View("AddUser");
            }

            return RedirectToRoute("Users-Login");
        }

        
        [HttpGet("Account", Name = "Users-Account")]
        public IActionResult UserAccount()
        {
            return View("UserAccount");
        }

    }
}
