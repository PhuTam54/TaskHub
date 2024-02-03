using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace TaskHub.Models.Authentication
{
    public class Authentication : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.HttpContext.Session.GetString("UserName") == null)
            {
                context.Result = new RedirectToRouteResult(
                    new RouteValueDictionary
                    {
                        {"Controller", "Users" },
                        {"Action","Login" }
                    });
            }
            else
            {
                string userRole = context.HttpContext.Session.GetString("Role");

                bool isRedirectedFromAdmin = context.HttpContext.Request.Headers["Referer"].ToString().Contains("Users/Login");

                if (userRole == "Admin" && isRedirectedFromAdmin && context.RouteData.Values["Controller"].ToString() != "Admin" && context.RouteData.Values["Action"].ToString() != "dashboard")
                {
                    context.Result = new NotFoundResult();
                }
                else if (userRole == "User" && context.RouteData.Values["Controller"].ToString() == "Admin")
                {
                    context.Result = new NotFoundResult();
                }
                else if (userRole == "User" && context.RouteData.Values["Controller"].ToString() != "Page" && context.RouteData.Values["Action"].ToString() != "home")
                {
                    context.Result = new RedirectToRouteResult(
                        new RouteValueDictionary
                        {
                           {"Controller", "Users" },
                           {"Action","Home" }
                        });
                }
            }
        }
    }
}