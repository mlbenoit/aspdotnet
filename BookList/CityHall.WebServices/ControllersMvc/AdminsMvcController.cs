using System;
using System.Threading.Tasks;
using BookRental.Services.Contracts;
using BookRental.WebContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookRental.WebServices.ControllersMvc
{
    [Route("admin/Admins")]
    [ApiExplorerSettings(IgnoreApi = true)]
   
    public class AdminsMvcController : MvcControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminsMvcController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [AllowAnonymous]
        [HttpGet("Login", Name ="Admins-Login")]
        public IActionResult Login()
        {
            return View("Login");
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login(AdminLoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ServiceResult<bool> result = await _adminService.LoginAsync(request);
            if (result.IsError)
            {
                ModelState.AddModelError("", "Invalid admin email or password");
                return View("Login");
            }

            return RedirectToRoute("Home-Index");
        }
    }
}
