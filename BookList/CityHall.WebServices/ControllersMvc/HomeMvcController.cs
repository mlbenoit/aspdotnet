using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace BookRental.WebServices.ControllersMvc
{
    [Route("admin/Home")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class HomeMvcController:MvcControllerBase
    {
        public HomeMvcController()
        {
        }

        [HttpGet("")]
        [HttpGet("Index", Name = "Home-Index")]
        public IActionResult Index()
        {
            return View("Index");
        }
    }
}
