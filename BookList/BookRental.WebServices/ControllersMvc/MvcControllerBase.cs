using System;
using System.Diagnostics;
using BookRental.WebServices.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookRental.WebServices.ControllersMvc
{
   [ApiExplorerSettings(IgnoreApi = true)]
   [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
   public abstract class MvcControllerBase : Controller
    {
        [ResponseCache(Duration =0, Location =ResponseCacheLocation.None, NoStore =true)]
        public IActionResult Error()
        {
            ErrorViewModel model = new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };
            return base.View(model);
        }
    }

}
