using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TaskHub.Data;
using TaskHub.MailUtils;
using TaskHub.Middleware;
using TaskHub.Models;
using TaskHub.Service;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddDbContext<TaskHubContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("TaskHubContext") ?? 
    throw new InvalidOperationException("Connection string 'TaskHubContext' not found.")));

builder.Services.AddSession();

//// Add configuration from appsettings.json
//builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
// Lấy cấu hình từ appsettings.json
var configuration = builder.Configuration;
// Lấy các cài đặt mail từ appsettings.json
var mailSettings = configuration.GetSection("MailSettings");
// Thêm cấu hình mail vào dịch vụ của ứng dụng
builder.Services.Configure<MailSettings>(mailSettings);

// SendMailService init
builder.Services.AddTransient<SendMailService>();

var app = builder.Build();

// Seed data
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    SeedData.Initialize(services);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseSession();
app.UseStaticFiles();

// Initial Middleware 
app.UseAdminMiddleware();

app.UseRouting(); // EndpointRoutingMiddleware

// Initial Enpoint ( Terminate middleware )
app.UseEndpoints((endpoints) =>
{
    //    endpoints.MapControllerRoute(
    //        name: "admin",
    //        pattern: "admin/{controller=Dashboard}/{action=Index}/{id?}");

    endpoints.MapGet("/TestSendMail", async context =>
    {
        var message = await MailUtils.SendMail("e", 
            "e", "Test sendMail", "This is a test email");
        await context.Response.WriteAsync(message);
    });

    endpoints.MapGet("/TestSendGmail", async context =>
    {
        var message = await MailUtils.SendGmail("e", 
            "e", "Test sendMail", "This is a test email",
            "e", "pwd");
        await context.Response.WriteAsync(message);
    });

    endpoints.MapGet("/TestSendMailService", async context =>
    {
        var sendMailService = context.RequestServices.GetService<SendMailService>();

        var mailContent = new MailContent();

        mailContent.To = "tamnpth2210002@fpt.edu.vn";
        mailContent.Subject = "Test send mail service";
        mailContent.Body = "<h1>Hello Phu Tam form sendMailService</h1>";

        var message = await sendMailService.SendMail(mailContent);
        await context.Response.WriteAsync(message);
    });
});

app.UseAuthorization();

app.MapRazorPages();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();