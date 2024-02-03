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

                // Kiểm tra nếu đang cố gắng chuyển hướng từ trang Admin
                bool isRedirectedFromAdmin = context.HttpContext.Request.Headers["Referer"].ToString().Contains("Admin/dashboard");

                if (userRole == "Admin" && isRedirectedFromAdmin && context.RouteData.Values["Controller"].ToString() != "Admin" && context.RouteData.Values["Action"].ToString() != "dashboard")
                {
                    // Nếu có cố gắng truy cập trái phép vào trang Admin, trả về mã lỗi 404
                    context.Result = new NotFoundResult();
                }
                else if (userRole == "User" && context.RouteData.Values["Controller"].ToString() == "Admin")
                {
                    // Nếu người dùng có vai trò là "User" cố gắng truy cập phần "Admin", trả về mã lỗi 404
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