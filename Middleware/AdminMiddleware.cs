using System.Linq;

namespace TaskHub.Middleware
{
    public class AdminMiddleware
    {
        private readonly RequestDelegate _next;
        // RequestDelegate ~ async (HttpContext context) => {}

        public AdminMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        // HttpContext di qua Middleware
        public async Task InvokeAsync(HttpContext context)
        {
            string[] listAdminPath = { 
                //"/Users", 
                //"/WorkSpaces",
                //"/Boards",
            };

            if (Array.Exists(listAdminPath, element => element == context.Request.Path))
            {
                var isAdmin = context.Session.GetString("isAdmin");
                if (isAdmin == "True")
                {
                    await _next(context);
                }
                return;

            }
            await _next(context);

        }
    }
}
