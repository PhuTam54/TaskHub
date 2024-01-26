using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TaskHub.Data;
using TaskHub.Models;

namespace TaskHub.Controllers
{
    public class AccountsController : Controller
    {
        private readonly TaskHubContext _context;


        public AccountsController(TaskHubContext context)
        {
            _context = context;
        }

        // GET: Accounts
        public async Task<IActionResult> Index()
        {
              return _context.Account != null ? 
                          View(await _context.Account.ToListAsync()) :
                          Problem("Entity set 'TaskHubContext.Account'  is null.");
        }

        // GET: Accounts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Account == null)
            {
                return NotFound();
            }

            var account = await _context.Account
                .FirstOrDefaultAsync(m => m.Id == id);
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }

        // GET: Accounts/Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: Accounts/Register
        [HttpPost]
        public ActionResult Register(Account account)
        {
            _context.Account.Add(account);
            _context.SaveChanges();
            return RedirectToAction("Login");
        }

        // Get: /Accounts/Login
        public ActionResult Login()
        {
            if (HttpContext.Session.GetString("Email") == null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        // Post: /Accounts/Login
        [HttpPost]
        public ActionResult Login(Account account)
        {
            if (HttpContext.Session.GetString("Email") == null)
            {
                var u = _context.Account.Where(x => x.Email.Equals(account.Email) && x.Password.Equals(account.Password)).FirstOrDefault();
                if (u != null)
                {
                    HttpContext.Session.SetString("Email", u.Email.ToString());
                    return RedirectToAction("Index", "Home");
                }
            }
            return View();

        }

        // Post: /Accounts/Logout
        public ActionResult Logout()
        {
            HttpContext.Session.Clear();
            HttpContext.Session.Remove("Accounts");
            return RedirectToAction("Index", "Home");
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,FullName,Email,Password")] Account account)
        {
            if (id != account.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(account);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccountExists(account.Id))
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
                .FirstOrDefaultAsync(m => m.Id == id);
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
          return (_context.Account?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
