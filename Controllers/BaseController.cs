using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace RentCar_AspNetCore7.Controllers
{
    public class BaseController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (HttpContext.Session.GetString("IsAdmin") != "True")
            {
                filterContext.Result = new RedirectToActionResult("Login", "Account", null);
            }
            base.OnActionExecuting(filterContext);
        }

    }
}
