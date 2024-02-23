using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TaskHub.Data;
using TaskHub.Models;
using TaskHub.Models.Authentication;

namespace TaskHub.Controllers
{
    [Authentication]
    public class AccountsController : Controller
    {
        private readonly TaskHubContext _context;

        public AccountsController(TaskHubContext context)
        {
            _context = context;
        }

        // GET: Accounts
        public async Task<IActionResult> Index(string searchString)
        {
            if (_context.Account == null)
            {
                return Problem("Entity set 'MvcMovieContext.Movie'  is null.");
            }

            var accounts = from m in _context.Account
                         select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                accounts = accounts.Where(s => s.Username!.Contains(searchString));
            }

            return View(await accounts.ToListAsync());
        }
        [Authentication]
        // GET: Accounts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Account == null)
            {
                return NotFound();
            }

            var account = await _context.Account
                .FirstOrDefaultAsync(m => m.ID == id);
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }

        // GET: Accounts/Create
        public IActionResult Register()
        {
            return View();
        }

        // POST: Accounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
      [HttpPost]
      [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("ID,Username,Email,Password")] Account account)
        {
            var existingEmail = await _context.Account.AnyAsync(a => a.Email == account.Email);
            if (existingEmail)
            {
                ModelState.AddModelError("Email", "Email already exists.");
                return View(account);
            }
            else if (ModelState.IsValid)
            {
                // Hash the password before saving it
                account.Password = BCrypt.Net.BCrypt.HashPassword(account.Password);
        
                _context.Add(account);
                await _context.SaveChangesAsync();
                return RedirectToAction("Login");
            }
            return View(account);
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
                return RedirectToAction("Index", "TaskItems");
            }
        }
        [HttpPost]
        public IActionResult Login(Account account)
        {
            if (HttpContext.Session.GetString("UserName") == null)
            {
                var user = _context.Account.Where(x => x.Email.Equals(account.Email)).FirstOrDefault();
                if (user != null && BCrypt.Net.BCrypt.Verify(account.Password, user.Password))
                {
                    HttpContext.Session.SetString("UserName", user.Email.ToString());
                    ViewBag.Username = user.Username;
                    return RedirectToAction("Index", "TaskItems");
                }
            }
            return View();
        }


        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            HttpContext.Session.Remove("UserName");
            return RedirectToAction("Login", "Accounts");
        }
        // GET: Accounts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Account == null)
            {
                return NotFound();
            }

            var account = await _context.Account.FindAsync(id);
            if (account == null)
            {
                return NotFound();
            }
            return View(account);
        }

        // POST: Accounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Username,Email,Password")] Account account)
        {
            if (id != account.ID)
            {
                return NotFound();
            }

            if (true)
            {
                try
                {
                    _context.Update(account);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccountExists(account.ID))
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
            return View(account);
        }

        // GET: Accounts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Account == null)
            {
                return NotFound();
            }

            var account = await _context.Account
                .FirstOrDefaultAsync(m => m.ID == id);
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }

        // POST: Accounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Account == null)
            {
                return Problem("Entity set 'TaskHubContext.Account'  is null.");
            }
            var account = await _context.Account.FindAsync(id);
            if (account != null)
            {
                _context.Account.Remove(account);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AccountExists(int id)
        {
          return (_context.Account?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
