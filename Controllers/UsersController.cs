using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TaskHub.Models;
using TaskHub.Data;
using TaskHub.Models.WorkSpaceViewModels;
using Microsoft.IdentityModel.Tokens;
using Microsoft.CodeAnalysis.Scripting;
using System.Security.Cryptography;
using System.Net;
using System.Net.Mail;
namespace TaskHub.Controllers
{
    public class UsersController : Controller
    {
        private readonly TaskHubContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public UsersController(TaskHubContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: Users/Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: Users/Register
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // POST: Users/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("ID,UserName,Email,Password,LastName,FirstMidName")] User user)
        {
            var existingEmail = await _context.User.AnyAsync(a => a.Email == user.Email);
            if (existingEmail)
            {
                ModelState.AddModelError("Email", "Email already exists.");
                return View(user);
            }

            // Generate a random reset password token
            string resetPasswordToken = GenerateRandomToken();

            // Hash the password before saving it
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            user.Avatar = "https://img.meta.com.vn/Data/image/2021/09/22/anh-meo-cute-de-thuong-dang-yeu-42.jpg";
            user.FirstMidName = user.UserName;
            user.LastName = "";
            user.UserRole = "User";
            user.ResetPasswordToken = resetPasswordToken;

            _context.Add(user);
            await _context.SaveChangesAsync();
            HttpContext.Session.SetString("UserName", user.UserName);
            HttpContext.Session.SetString("UserRole", user.UserRole);
            return RedirectToAction("Login");
        }

        // Generate a random reset password token
        private string GenerateRandomToken()
        {
            // Generate a random token here
            return Guid.NewGuid().ToString();
        }

        [HttpGet]
        public IActionResult Error()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("UserName") == null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("MyBoards", "Home");
            }
        }
        [HttpPost]
        public IActionResult Login(User user)
        {
            if (HttpContext.Session.GetString("UserName") == null)
            {
                var account = _context.User.Where(x => x.Email.Equals(user.Email)).FirstOrDefault();

                if (account != null && BCrypt.Net.BCrypt.Verify(user.Password, account.Password))
                {
                    HttpContext.Session.SetInt32("UserID", account.ID);
                    HttpContext.Session.SetString("UserName", account.UserName);
                    HttpContext.Session.SetString("Avatar", account.Avatar);
                    HttpContext.Session.SetString("UserName", account.UserName.ToString());
                    HttpContext.Session.SetString("UserRole", account.UserRole);

                    ViewBag.Username = account.UserName;
                    ViewBag.UserId = account.ID;
                    ViewBag.Avatar = account.Avatar;

                    if (account.UserRole == "Admin" || account.UserRole == "User")
                    {
                        return RedirectToAction("MyBoards", "Home");
                    }
                    return View();
                }
            }

            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            HttpContext.Session.Remove("UserId");
            HttpContext.Session.Remove("UserName");
            return RedirectToAction("Login", "Users");
        }
        // GET: Users
        public async Task<IActionResult> Index(int? id)
        {
            var viewModel = new UserIndexData();
            viewModel.Users = await _context.User
                .Include(i => i.WorkSpaceMembers)
                    .ThenInclude(i => i.WorkSpace)
                .Include(i => i.WorkSpaces)
                .AsNoTracking()
                .OrderByDescending(i => i.ID)
                .ToListAsync();

            if (id != null)
            {
                ViewData["UserID"] = id.Value;
                User user = viewModel.Users.Where(
                    i => i.ID == id.Value).Single();
                viewModel.WorkSpaces = user.WorkSpaces;
            }

            return View(viewModel);
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.User == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.ID == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,UserName,Email,Password,LastName,FirstMidName")] User user, IFormFile Avatar)
        {
            if (true)
            {
                var allowedExtenstions = new[] { ".jpg", ".jpeg", ".png", ".gif" };

                var filePaths = new List<string>();
                // Check if the file has a valid extensions
                var fileExtension = Path.GetExtension(Avatar.FileName).ToLowerInvariant();
                if (string.IsNullOrEmpty(fileExtension) || !allowedExtenstions.Contains(fileExtension))
                {
                    return BadRequest("Invalid file extension. Allowed extensions are: " + string.Join(", ", allowedExtenstions));
                };

                if (Avatar.Length > 0)
                {
                    // Change the folder path
                    var uploadFolderPath = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
                    Directory.CreateDirectory(uploadFolderPath);

                    var fileName = Path.GetRandomFileName() + fileExtension;
                    var filePath = Path.Combine(uploadFolderPath, fileName);
                    filePaths.Add(filePath);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await Avatar.CopyToAsync(stream);
                    }

                    user.Avatar = "/uploads/" + fileName;
                }
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Users/ForgotPassword
        public IActionResult ForgotPassword()
        {
            return View();
        }

        // POST: Users/ForgotPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(string email, User users)
        {
            if (true)
            {
                ModelState.Clear();
                users.EmailSend = true;

                if (string.IsNullOrEmpty(email))
                {
                    ModelState.AddModelError("Email", "Email is required.");
                    return View();
                }

                var user = await _context.User.FirstOrDefaultAsync(u => u.Email == email);
                if (user == null)
                {
                    ModelState.AddModelError("Email", "User with this email does not exist.");
                    return View();
                }

                var token = GenerateToken();
                user.ResetPasswordToken = token;
                user.ResetPasswordTokenExpiration = DateTime.UtcNow.AddHours(1);
                await _context.SaveChangesAsync();

                var callbackUrl = Url.Action("ResetPassword", "Users", new { token = token }, protocol: HttpContext.Request.Scheme);
                await SendResetPasswordEmail(email, callbackUrl);

                TempData["Message"] = "Reset password link has been sent to your email.";
            }
           
            return RedirectToAction("ForgotPassword");
        }
        public async Task SendResetPasswordEmail(string email, string callbackUrl)
        {
            var mailMessage = new MailMessage();
            mailMessage.To.Add(email);
            mailMessage.Subject = "Reset Password";
            mailMessage.From = new MailAddress(email);
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = $"Please reset your password by <a href='{callbackUrl}'>clicking here</a>.";

            using (var smtpClient = new SmtpClient("smtp.gmail.com"))
            {
                smtpClient.Port = 587;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.EnableSsl = true;
                smtpClient.Credentials = new NetworkCredential("locnvth2209036@fpt.edu.vn", "facuytnqgsyxupdl");

                await smtpClient.SendMailAsync(mailMessage);
            }
        }

        // GET: Users/ResetPassword
        public async Task<IActionResult> ResetPassword(string token)
        {
            var user = await _context.User.FirstOrDefaultAsync(u => u.ResetPasswordToken == token && u.ResetPasswordTokenExpiration > DateTime.UtcNow);
            if (user == null)
            {
                TempData["ErrorMessage"] = "Invalid or expired reset password token.";
                return RedirectToAction(nameof(Login));
            }

            var viewModel = new ResetPasswordViewModel
            {
                Token = token
            };
            return View(viewModel);
        }

        // POST: Users/ResetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var user = await _context.User.FirstOrDefaultAsync(u => u.ResetPasswordToken == viewModel.Token && u.ResetPasswordTokenExpiration > DateTime.UtcNow);
            if (user == null)
            {
                TempData["ErrorMessage"] = "Invalid or expired reset password token.";
                return RedirectToAction(nameof(Login));
            }

            user.Password = BCrypt.Net.BCrypt.HashPassword(viewModel.NewPassword);
            user.ResetPasswordToken = "";
            user.ResetPasswordTokenExpiration = null;
            await _context.SaveChangesAsync();

            TempData["Message"] = "Password has been reset successfully.";
            return RedirectToAction(nameof(Login));
        }

        private string GenerateToken()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                var tokenData = new byte[32];
                rng.GetBytes(tokenData);
                return Convert.ToBase64String(tokenData);
            }
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.User == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,UserName,Email,Password,LastName,FirstMidName")] User user, IFormFile Avatar)
        {
            if (id != user.ID)
            {
                return NotFound();
            }

            if (true)
            {
                try
                {
                    var allowedExtenstions = new[] { ".jpg", ".jpeg", ".png", ".gif" };

                    var filePaths = new List<string>();
                    // Check if the file has a valid extensions
                    var fileExtension = Path.GetExtension(Avatar.FileName).ToLowerInvariant();
                    if (string.IsNullOrEmpty(fileExtension) || !allowedExtenstions.Contains(fileExtension))
                    {
                        return BadRequest("Invalid file extension. Allowed extensions are: " + string.Join(", ", allowedExtenstions));
                    };

                    if (Avatar.Length > 0)
                    {
                        // Change the folder path
                        var uploadFolderPath = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
                        Directory.CreateDirectory(uploadFolderPath);

                        var fileName = Path.GetRandomFileName() + fileExtension;
                        var filePath = Path.Combine(uploadFolderPath, fileName);
                        filePaths.Add(filePath);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await Avatar.CopyToAsync(stream);
                        }

                        user.Avatar = "/uploads/" + fileName;
                    }
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.User == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.User == null)
            {
                return Problem("Entity set 'TaskHubContext.User'  is null.");
            }
            var user = await _context.User.FindAsync(id);
            if (user != null)
            {
                _context.User.Remove(user);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return (_context.User?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}