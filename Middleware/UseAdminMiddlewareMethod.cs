namespace TaskHub.Middleware
{
    public static class UseAdminMiddlewareMethod
    {
        public static void UseAdminMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<AdminMiddleware>();
        }
    }
}
