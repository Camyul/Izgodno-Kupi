using Microsoft.AspNetCore.Mvc;

namespace IzgodnoKupi.Web.Controllers
{
    public class ErrorController : Controller
    {
        [Route("error/404")]
        public IActionResult Error404()
        {
            //return the default error view
            return View();
        }

        [Route("error/{code:int}")]
        public IActionResult Error(int code)
        {
            // handle different codes or just return the default error view
            return View("Error404");
        }
    }
}
