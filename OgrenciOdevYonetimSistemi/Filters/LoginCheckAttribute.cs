using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace OgrenciOdevYonetimSistemi.Filters
{
    public class LoginCheckAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var httpContext = context.HttpContext;
            var session = httpContext.Session;

            var ogrenciNo = session.GetString("OgrenciNo");
            var ogretmenAd = session.GetString("OgretmenAd");

            var controllerName = context.RouteData.Values["controller"]?.ToString();

            if (controllerName == "Ogrenci" && string.IsNullOrEmpty(ogrenciNo))
            {
                context.Result = new RedirectToActionResult("OgrenciLogin", "Account", null);
            }
            else if (controllerName == "Ogretmen" && string.IsNullOrEmpty(ogretmenAd))
            {
                context.Result = new RedirectToActionResult("OgretmenLogin", "Account", null);
            }
        }
    }
}
