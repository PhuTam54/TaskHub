using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TaskHub.Data;
using TaskHub.MailUtils;
using TaskHub.Models;
using TaskHub.Service;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddDbContext<TaskHubContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("TaskHubContext") ?? 
    throw new InvalidOperationException("Connection string 'TaskHubContext' not found.")));
builder.Services.AddHttpContextAccessor();
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

app.UseRouting(); // EndpointRoutingMiddleware

// Initial Enpoint ( Terminate middleware )
app.UseEndpoints((endpoints) =>
{
    endpoints.MapGet("/SendMailService/{email}", async context =>
    {
        var dbContext = context.RequestServices.GetService<TaskHubContext>(); // Inject DbContext

        var email = context.Request.RouteValues["email"]?.ToString();

        // Kiểm tra xem email có tồn tại không
        if (string.IsNullOrEmpty(email))
        {
            await context.Response.WriteAsync("Invalid email address");
            return;
        }
        // Thực hiện truy vấn để lấy ra người dùng với địa chỉ email đã cung cấp
        var user = await dbContext.User.FirstOrDefaultAsync(u => u.Email == email);
        // Kiểm tra xem người dùng có tồn tại không
        if (user == null)
        {
            await context.Response.WriteAsync($"User with email '{email}' not found");
            return;
        }

        var userInvite = context.Request.HttpContext.Session.GetString("UserName");
        var workSpaceInvite = context.Request.HttpContext.Session.GetInt32("WorkSpaceID");
        var userToInvite = user.ID;
        var enrollmentDate = DateTime.Now;

        var sendMailService = context.RequestServices.GetService<SendMailService>();

        var mailContent = new MailContent();

        mailContent.To = $"{email}";
        mailContent.Subject = "Invite mail";
        mailContent.Body = $"<!DOCTYPE html>\r\n<html lang=\"en\" xmlns=\"http://www.w3.org/1999/xhtml\" xmlns:v=\"urn:schemas-microsoft-com:vml\" xmlns:o=\"urn:schemas-microsoft-com:office:office\">\r\n<head>\r\n    <meta charset=\"utf-8\"> <!-- utf-8 works for most cases -->\r\n    <meta name=\"viewport\" content=\"width=device-width\"> <!-- Forcing initial-scale shouldn't be necessary -->\r\n    <meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\"> <!-- Use the latest (edge) version of IE rendering engine -->\r\n    <meta name=\"x-apple-disable-message-reformatting\">  <!-- Disable auto-scale in iOS 10 Mail entirely -->\r\n    <title></title> <!-- The title tag shows in email notifications, like Android 4.4. -->\r\n\r\n    <link href=\"https://fonts.googleapis.com/css?family=Open+Sans:200,300,400,500,600,700\" rel=\"stylesheet\">\r\n\r\n    <!-- CSS Reset : BEGIN -->\r\n    <style>\r\n\r\n        /* What it does: Remove spaces around the email design added by some email clients. */\r\n        /* Beware: It can remove the padding / margin and add a background color to the compose a reply window. */\r\n        html,\r\n        body {{\r\n            margin: 0 auto !important;\r\n            padding: 0 !important;\r\n            height: 100% !important;\r\n            width: 100% !important;\r\n            background: #f1f1f1;\r\n        }}\r\n\r\n        /* What it does: Stops email clients resizing small text. */\r\n        * {{\r\n            -ms-text-size-adjust: 100%;\r\n            -webkit-text-size-adjust: 100%;\r\n        }}\r\n\r\n        /* What it does: Centers email on Android 4.4 */\r\n        div[style*=\"margin: 16px 0\"] {{\r\n            margin: 0 !important;\r\n        }}\r\n\r\n        /* What it does: Stops Outlook from adding extra spacing to tables. */\r\n        table,\r\n        td {{\r\n            mso-table-lspace: 0pt !important;\r\n            mso-table-rspace: 0pt !important;\r\n        }}\r\n\r\n        /* What it does: Fixes webkit padding issue. */\r\n        table {{\r\n            border-spacing: 0 !important;\r\n            border-collapse: collapse !important;\r\n            table-layout: fixed !important;\r\n            margin: 0 auto !important;\r\n        }}\r\n\r\n        /* What it does: Uses a better rendering method when resizing images in IE. */\r\n        img {{\r\n            -ms-interpolation-mode:bicubic;\r\n        }}\r\n\r\n        /* What it does: Prevents Windows 10 Mail from underlining links despite inline CSS. Styles for underlined links should be inline. */\r\n        a {{\r\n            text-decoration: none;\r\n        }}\r\n\r\n        /* What it does: A work-around for email clients meddling in triggered links. */\r\n        *[x-apple-data-detectors],  /* iOS */\r\n        .unstyle-auto-detected-links *,\r\n        .aBn {{\r\n            border-bottom: 0 !important;\r\n            cursor: default !important;\r\n            color: inherit !important;\r\n            text-decoration: none !important;\r\n            font-size: inherit !important;\r\n            font-family: inherit !important;\r\n            font-weight: inherit !important;\r\n            line-height: inherit !important;\r\n        }}\r\n\r\n        /* What it does: Prevents Gmail from displaying a download button on large, non-linked images. */\r\n        .a6S {{\r\n            display: none !important;\r\n            opacity: 0.01 !important;\r\n        }}\r\n\r\n        /* What it does: Prevents Gmail from changing the text color in conversation threads. */\r\n        .im {{\r\n            color: inherit !important;\r\n        }}\r\n\r\n        /* If the above doesn't work, add a .g-img class to any image in question. */\r\n        img.g-img + div {{\r\n            display: none !important;\r\n        }}\r\n\r\n        /* What it does: Removes right gutter in Gmail iOS app: https://github.com/TedGoas/Cerberus/issues/89  */\r\n        /* Create one of these media queries for each additional viewport size you'd like to fix */\r\n\r\n        /* iPhone 4, 4S, 5, 5S, 5C, and 5SE */\r\n        @media only screen and (min-device-width: 320px) and (max-device-width: 374px) {{\r\n            u ~ div .email-container {{\r\n                min-width: 320px !important;\r\n            }}\r\n        }}\r\n        /* iPhone 6, 6S, 7, 8, and X */\r\n        @media only screen and (min-device-width: 375px) and (max-device-width: 413px) {{\r\n            u ~ div .email-container {{\r\n                min-width: 375px !important;\r\n            }}\r\n        }}\r\n        /* iPhone 6+, 7+, and 8+ */\r\n        @media only screen and (min-device-width: 414px) {{\r\n            u ~ div .email-container {{\r\n                min-width: 414px !important;\r\n            }}\r\n        }}\r\n    </style>\r\n\r\n    <!-- CSS Reset : END -->\r\n\r\n    <!-- Progressive Enhancements : BEGIN -->\r\n    <style>\r\n\r\n        .primary{{\r\n            background: #ff4f81;\r\n        }}\r\n        .bg_white{{\r\n            background: #ffffff;\r\n        }}\r\n        .bg_light{{\r\n            background: #f7fafa;\r\n        }}\r\n        .bg_black{{\r\n            background: #000000;\r\n        }}\r\n        .bg_dark{{\r\n            background: rgba(0,0,0,.8);\r\n        }}\r\n        .email-section{{\r\n            padding:2.5em;\r\n        }}\r\n\r\n        /*BUTTON*/\r\n        .btn{{\r\n            padding: 10px 15px;\r\n            display: inline-block;\r\n        }}\r\n        .btn.btn-primary{{\r\n            border-radius: 5px;\r\n            background: #ff4f81;\r\n            color: #ffffff;\r\n        }}\r\n        .btn.btn-white{{\r\n            border-radius: 5px;\r\n            background: #ffffff;\r\n            color: #000000;\r\n        }}\r\n        .btn.btn-white-outline{{\r\n            border-radius: 5px;\r\n            background: transparent;\r\n            border: 1px solid #fff;\r\n            color: #fff;\r\n        }}\r\n        .btn.btn-black-outline{{\r\n            border-radius: 0px;\r\n            background: transparent;\r\n            border: 2px solid #000;\r\n            color: #000;\r\n            font-weight: 700;\r\n        }}\r\n        .btn-custom{{\r\n            color: rgba(0,0,0,.3);\r\n            text-decoration: underline;\r\n        }}\r\n\r\n        h1,h2,h3,h4,h5,h6{{\r\n            font-family: 'Work Sans', sans-serif;\r\n            color: #000000;\r\n            margin-top: 0;\r\n            font-weight: 400;\r\n        }}\r\n\r\n        body{{\r\n            font-family: 'Work Sans', sans-serif;\r\n            font-weight: 400;\r\n            font-size: 15px;\r\n            line-height: 1.8;\r\n            color: rgba(0,0,0,.4);\r\n        }}\r\n\r\n        a{{\r\n            color: #ff4f81;\r\n        }}\r\n\r\n        table{{\r\n        }}\r\n        /*LOGO*/\r\n\r\n        .logo h1{{\r\n            margin: 0;\r\n        }}\r\n        .logo h1 a{{\r\n            color: #ff4f81;\r\n            font-size: 24px;\r\n            font-weight: 700;\r\n            font-family: 'Work Sans', sans-serif;\r\n        }}\r\n\r\n        /*HERO*/\r\n        .hero{{\r\n            position: relative;\r\n            z-index: 0;\r\n        }}\r\n\r\n        .hero .text{{\r\n            color: rgba(0,0,0,.3);\r\n        }}\r\n        .hero .text h2{{\r\n            color: #000;\r\n            font-size: 34px;\r\n            margin-bottom: 15px;\r\n            font-weight: 300;\r\n            line-height: 1.2;\r\n        }}\r\n        .hero .text h3{{\r\n            font-size: 24px;\r\n            font-weight: 200;\r\n        }}\r\n        .hero .text h2 span{{\r\n            font-weight: 600;\r\n            color: #000;\r\n        }}\r\n\r\n\r\n        /*PRODUCT*/\r\n        .product-entry{{\r\n            display: block;\r\n            position: relative;\r\n            float: left;\r\n            padding-top: 20px;\r\n        }}\r\n        .product-entry .text{{\r\n            width: calc(100% - 125px);\r\n            padding-left: 20px;\r\n        }}\r\n        .product-entry .text h3{{\r\n            margin-bottom: 0;\r\n            padding-bottom: 0;\r\n        }}\r\n        .product-entry .text p{{\r\n            margin-top: 0;\r\n        }}\r\n        .product-entry img, .product-entry .text{{\r\n            float: left;\r\n        }}\r\n\r\n        ul.social{{\r\n            padding: 0;\r\n        }}\r\n        ul.social li{{\r\n            display: inline-block;\r\n            margin-right: 10px;\r\n        }}\r\n\r\n        /*FOOTER*/\r\n\r\n        .footer{{\r\n            border-top: 1px solid rgba(0,0,0,.05);\r\n            color: rgba(0,0,0,.5);\r\n        }}\r\n        .footer .heading{{\r\n            color: #000;\r\n            font-size: 20px;\r\n        }}\r\n        .footer ul{{\r\n            margin: 0;\r\n            padding: 0;\r\n        }}\r\n        .footer ul li{{\r\n            list-style: none;\r\n            margin-bottom: 10px;\r\n        }}\r\n        .footer ul li a{{\r\n            color: rgba(0,0,0,1);\r\n        }}\r\n\r\n\r\n        @media screen and (max-width: 500px) {{\r\n\r\n\r\n        }}\r\n\r\n\r\n    </style>\r\n\r\n\r\n</head>\r\n\r\n<body width=\"100%\" style=\"margin: 0; padding: 0 !important; mso-line-height-rule: exactly; background-color: #f1f1f1;\">\r\n<center style=\"width: 100%; background-color: #f1f1f1;\">\r\n    <div style=\"display: none; font-size: 1px;max-height: 0px; max-width: 0px; opacity: 0; overflow: hidden; mso-hide: all; font-family: sans-serif;\">\r\n        &zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;\r\n    </div>\r\n    <div style=\"max-width: 600px; margin: 0 auto;\" class=\"email-container\">\r\n        <!-- BEGIN BODY -->\r\n        <table align=\"center\" role=\"presentation\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" width=\"100%\" style=\"margin: auto;\">\r\n            <tr>\r\n                <td valign=\"top\" class=\"bg_white\" style=\"padding: 1em 2.5em 0 2.5em;\">\r\n                    <table role=\"presentation\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">\r\n                        <tr>\r\n                            <td class=\"logo\" style=\"text-align: left;\">\r\n                                <h1><a href=\"#\">TaskHub</a></h1>\r\n                            </td>\r\n                        </tr>\r\n                    </table>\r\n                </td>\r\n            </tr><!-- end tr -->\r\n            <tr>\r\n                <td valign=\"middle\" class=\"hero bg_white\" style=\"padding: 2em 0 2em 0;\">\r\n                    <table role=\"presentation\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">\r\n                        <tr>\r\n                            <td style=\"padding: 0 2.5em; text-align: left;\">\r\n                                <div class=\"text\">\r\n                                    <h2>Hello {email} form sendMailService</h2>\r\n                                    <h3>{userInvite} want to invite you to a WorkSpace {workSpaceInvite}</h3>\r\n <h3>Wanna join?</h3>\r\n                                </div>\r\n                            </td>\r\n                        </tr>\r\n                    </table>\r\n                </td>\r\n            </tr><!-- end tr -->\r\n            <tr>\r\n                <table class=\"bg_white\" role=\"presentation\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">\r\n                    <tr>\r\n                        <td valign=\"middle\" width=\"100%\" style=\"text-align:left; padding: 1em 2.5em;\">\r\n                            <p><a href='https://localhost:7253/Home/JoinWorkSpace?UserId={{userToInvite}}&WorkSpaceId={{workSpaceInvite}}&EnrollmentDate={{enrollmentDate}}&Status=1' class=\"btn btn-primary\">Click this button</a></p>\r\n                        </td>\r\n                    </tr>\r\n                </table>\r\n            </tr><!-- end tr -->\r\n            <!-- 1 Column Text + Button : END -->\r\n        </table>\r\n        <table align=\"center\" role=\"presentation\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" width=\"100%\" style=\"margin: auto;\">\r\n            <tr>\r\n                <td valign=\"middle\" class=\"bg_light footer email-section\">\r\n                    <table>\r\n                        <tr>\r\n                            <td valign=\"top\" width=\"33.333%\" style=\"padding-top: 20px;\">\r\n                                <table role=\"presentation\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" width=\"100%\">\r\n                                    <tr>\r\n                                        <td style=\"text-align: left; padding-right: 10px;\">\r\n                                            <h3 class=\"heading\">About</h3>\r\n                                            <p>\"Unlock your potential in the digital arena. Success is just a click away.\"</p>\r\n                                        </td>\r\n                                    </tr>\r\n                                </table>\r\n                            </td>\r\n                            <td valign=\"top\" width=\"33.333%\" style=\"padding-top: 20px;\">\r\n                                <table role=\"presentation\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" width=\"100%\">\r\n                                    <tr>\r\n                                        <td style=\"text-align: left; padding-left: 5px; padding-right: 5px;\">\r\n                                            <h3 class=\"heading\">Contact Info</h3>\r\n                                            <ul>\r\n                                                <li><span class=\"text\">8A Tôn Thất Thuyết, Mỹ Đình, Nam Từ Liêm, Hà Nội</span></li>\r\n                                                <li><span class=\"text\">+84 65630471</span></li>\r\n                                            </ul>\r\n                                        </td>\r\n                                    </tr>\r\n                                </table>\r\n                            </td>\r\n                            <td valign=\"top\" width=\"33.333%\" style=\"padding-top: 20px;\">\r\n                                <table role=\"presentation\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" width=\"100%\">\r\n                                    <tr>\r\n                                        <td style=\"text-align: left; padding-left: 10px;\">\r\n                                            <h3 class=\"heading\">Useful Links</h3>\r\n                                            <ul>\r\n                                                <li><a href=\"#\">Home</a></li>\r\n                                                <li><a href=\"#\">Exam</a></li>\r\n                                                <li><a href=\"#\">Result</a></li>\r\n                                            </ul>\r\n                                        </td>\r\n                                    </tr>\r\n                                </table>\r\n                            </td>\r\n                        </tr>\r\n                    </table>\r\n                </td>\r\n            </tr><!-- end: tr -->\r\n        </table>\r\n\r\n    </div>\r\n</center>\r\n</body>\r\n</html>\r\n";

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