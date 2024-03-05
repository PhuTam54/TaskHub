using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskHub.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TaskHub.Data;
using TaskHub.Models;
using TaskHub.Models.WorkSpaceViewModels;
using Microsoft.AspNetCore.Http;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography;

namespace TaskHub.Controllers
{
    public class Home : Controller
    {
        private readonly TaskHubContext _context;

        public Home(TaskHubContext context)
        {
            _context = context;
        }

        // GET: Home
        public async Task<IActionResult> Index(int workSpaceId = 1)
        {
            var memberId = 1;
            var viewModel = new WorkSpaceIndexData();
            viewModel.WorkSpaces = await _context.WorkSpace
                .Include(i => i.User)
                .Include(i => i.WorkSpaceMembers)
                    .ThenInclude(i => i.User)
                .Include(i => i.Boards)
                    .ThenInclude(i => i.Lists)
                        .ThenInclude(i => i.TaskItems)
                            .ThenInclude(i => i.Comments)
                .AsNoTracking()
                .Where(i => i.WorkSpaceMembers.Any(wm => wm.MemberId == memberId))
                .ToListAsync();

            return View(viewModel);
        }

        // GET: Home/MyBoards/:WorkSpaceId
        public async Task<IActionResult> MyBoards(int id)
        {
            ViewBag.WorkSpaceId = id;

            if (id > 0)
            {
                var workSpace = await _context.WorkSpace.FirstOrDefaultAsync(w => w.WorkSpaceId == id);
                HttpContext.Session.SetString("WorkSpaceTitle", workSpace.WorkSpaceTitle);
            }

            HttpContext.Session.SetInt32("WorkSpaceID", id);
            var userId = HttpContext.Session.GetInt32("UserID");
            var userName = HttpContext.Session.GetString("UserName");
            var userAvatar = HttpContext.Session.GetString("Avatar");

            var viewModel = new WorkSpaceIndexData();
            viewModel.WorkSpaces = await _context.WorkSpace
                .Include(i => i.User)
                .Include(i => i.WorkSpaceMembers)
                    .ThenInclude(i => i.User)
                .Include(i => i.Boards.OrderBy(b => b.BoardId))
                    .ThenInclude(i => i.Lists)
                        .ThenInclude(i => i.TaskItems.OrderBy(ti => ti.position))
                            .ThenInclude(i => i.Comments)
                .AsNoTracking()
                .Where(i => i.WorkSpaceMembers.Any(wm => wm.User.ID == userId))
                .ToListAsync();

            ViewBag.DataFromDatabase = viewModel.WorkSpaces;
            ViewBag.Username = userName;
            ViewBag.UserId = userId;
            ViewBag.Avatar = userAvatar;
            return View(viewModel);
        }

        // User invited to a WorkSpace
        [HttpGet]
        public async Task<IActionResult> JoinWorkSpace([Bind("WorkSpaceId,UserId,EnrollmentDate,Status")] WorkSpaceMember workSpaceMember)
        {
            _context.Add(workSpaceMember);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(MyBoards));
        }

        // Login / Register
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
            return RedirectToAction("Login", "Home");
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

        // CRUD CALL API

        // POST: Home/CreateBoard
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateBoard([FromBody] Board board)
        {
            if (true)
            {
                _context.Add(board);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(MyBoards));
            }
        }

        // PUT: Home/EditBoard/{id}
        [HttpPut]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditBoard(int id, [FromBody] Board board)
        {
            if (id != board.BoardId)
            {
                return NotFound();
            }

            if (true)
            {
                try
                {
                    _context.Update(board);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BoardExists(board.BoardId))
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
        }

        // DELETE: Home/DeleteBoard/{id}
        [HttpDelete, ActionName("DeleteBoard")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Board == null)
            {
                return Problem("Entity set 'TaskHubContext.Board'  is null.");
            }
            var board = await _context.Board.FindAsync(id);
            if (board != null)
            {
                _context.Board.Remove(board);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool BoardExists(int id)
        {
            return (_context.Board?.Any(e => e.BoardId == id)).GetValueOrDefault();
        }

        // POST: Home/CreateList
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateList([FromBody] List list)
        {
            if (true)
            {
                _context.Add(list);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
        }

        // PUT: Home/EditList/{id}
        [HttpPut]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditList(int id, [FromBody] List list)
        {
            if (id != list.ListId)
            {
                return NotFound();
            }

            if (true)
            {
                try
                {
                    _context.Update(list);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ListExists(list.ListId))
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
        }

        // DELETE: Home/DeleteList/{id}
        [HttpDelete]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteList(int id)
        {
            if (_context.List == null)
            {
                return Problem("Entity set 'TaskHubContext.List'  is null.");
            }
            var list = await _context.List.FindAsync(id);
            if (list != null)
            {
                _context.List.Remove(list);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool ListExists(int id)
        {
            return (_context.List?.Any(e => e.ListId == id)).GetValueOrDefault();
        }

        // POST: Home/CreateTaskItem
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTaskItem([FromBody] TaskItem taskItem)
        {
            if (true)
            {
                taskItem.Deadline = DateTime.Now;
                _context.Add(taskItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
        }

        // PUT: Home/EditTaskItem/{id}
        [HttpPut]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTaskItem(int id, [FromBody] TaskItem taskItem)
        {
            if (id != taskItem.Id)
            {
                return NotFound();
            }

            if (true)
            {
                try
                {
                    _context.Update(taskItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaskItemExists(taskItem.Id))
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
        }

        // DELETE: Home/DeleteTaskItem/{id}
        [HttpDelete]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteTaskItem(int id)
        {
            if (_context.TaskItem == null)
            {
                return Problem("Entity set 'TaskHubContext.TaskItem'  is null.");
            }
            var taskItem = await _context.TaskItem.FindAsync(id);
            if (taskItem != null)
            {
                _context.TaskItem.Remove(taskItem);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool TaskItemExists(int id)
        {
            return (_context.TaskItem?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
