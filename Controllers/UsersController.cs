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
