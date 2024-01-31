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
    public class WorkSpaceMembersController : Controller
    {
        private readonly TaskHubContext _context;

        public WorkSpaceMembersController(TaskHubContext context)
        {
            _context = context;
        }

        // GET: WorkSpaceMembers
        public async Task<IActionResult> Index()
        {
            var taskHubContext = _context.WorkSpaceMember.Include(w => w.User).Include(w => w.WorkSpace);
            return View(await taskHubContext.ToListAsync());
        }

        // GET: WorkSpaceMembers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.WorkSpaceMember == null)
            {
                return NotFound();
            }

            var workSpaceMember = await _context.WorkSpaceMember
                .Include(w => w.User)
                .Include(w => w.WorkSpace)
                .FirstOrDefaultAsync(m => m.MemberId == id);
            if (workSpaceMember == null)
            {
                return NotFound();
            }

            return View(workSpaceMember);
        }

        // GET: WorkSpaceMembers/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.User, "ID", "ID");
            ViewData["WorkSpaceId"] = new SelectList(_context.WorkSpace, "WorkSpaceId", "WorkSpaceDescription");
            return View();
        }

        // POST: WorkSpaceMembers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MemberId,WorkSpaceId,UserId,EnrollmentDate,Status")] WorkSpaceMember workSpaceMember)
        {
            if (true)
            {
                _context.Add(workSpaceMember);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.User, "ID", "ID", workSpaceMember.UserId);
            ViewData["WorkSpaceId"] = new SelectList(_context.WorkSpace, "WorkSpaceId", "WorkSpaceDescription", workSpaceMember.WorkSpaceId);
            return View(workSpaceMember);
        }

        // GET: WorkSpaceMembers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.WorkSpaceMember == null)
            {
                return NotFound();
            }

            var workSpaceMember = await _context.WorkSpaceMember.FindAsync(id);
            if (workSpaceMember == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.User, "ID", "ID", workSpaceMember.UserId);
            ViewData["WorkSpaceId"] = new SelectList(_context.WorkSpace, "WorkSpaceId", "WorkSpaceDescription", workSpaceMember.WorkSpaceId);
            return View(workSpaceMember);
        }

        // POST: WorkSpaceMembers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MemberId,WorkSpaceId,UserId,EnrollmentDate,Status")] WorkSpaceMember workSpaceMember)
        {
            if (id != workSpaceMember.MemberId)
            {
                return NotFound();
            }

            if (true)
            {
                try
                {
                    _context.Update(workSpaceMember);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WorkSpaceMemberExists(workSpaceMember.MemberId))
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
            ViewData["UserId"] = new SelectList(_context.User, "ID", "ID", workSpaceMember.UserId);
            ViewData["WorkSpaceId"] = new SelectList(_context.WorkSpace, "WorkSpaceId", "WorkSpaceDescription", workSpaceMember.WorkSpaceId);
            return View(workSpaceMember);
        }

        // GET: WorkSpaceMembers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.WorkSpaceMember == null)
            {
                return NotFound();
            }

            var workSpaceMember = await _context.WorkSpaceMember
                .Include(w => w.User)
                .Include(w => w.WorkSpace)
                .FirstOrDefaultAsync(m => m.MemberId == id);
            if (workSpaceMember == null)
            {
                return NotFound();
            }

            return View(workSpaceMember);
        }

        // POST: WorkSpaceMembers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.WorkSpaceMember == null)
            {
                return Problem("Entity set 'TaskHubContext.WorkSpaceMember'  is null.");
            }
            var workSpaceMember = await _context.WorkSpaceMember.FindAsync(id);
            if (workSpaceMember != null)
            {
                _context.WorkSpaceMember.Remove(workSpaceMember);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WorkSpaceMemberExists(int id)
        {
          return (_context.WorkSpaceMember?.Any(e => e.MemberId == id)).GetValueOrDefault();
        }
    }
}
