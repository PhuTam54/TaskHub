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


namespace TaskHub.Controllers
{
    public class WorkSpacesController : Controller
    {
        private readonly TaskHubContext _context;

        public WorkSpacesController(TaskHubContext context)
        {
            _context = context;
        }

        // GET: WorkSpaces
        public async Task<IActionResult> Index()
        {

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
                .OrderBy(i => i.WorkSpaceId)
                .ToListAsync();

            return View(viewModel);

        }

        // GET: WorkSpaces/Details/5
        public async Task<IActionResult> Details(int? id)
        {

            ViewBag.WorkSpaceId = id;
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
                .OrderBy(i => i.WorkSpaceId)
                .ToListAsync();

            return View(viewModel);

        }

        // GET: WorkSpaces/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.User, "ID", "ID");
            return View();
        }

        // POST: WorkSpaces/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("WorkSpaceId,WorkSpaceTitle,WorkSpaceDescription,Status,UserId")] WorkSpace workSpace)
        {
            if (true)
            {
                _context.Add(workSpace);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.User, "ID", "ID", workSpace.UserId);
            return View(workSpace);
        }

        // GET: WorkSpaces/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.WorkSpace == null)
            {
                return NotFound();
            }

            var workSpace = await _context.WorkSpace.FindAsync(id);
            if (workSpace == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.User, "ID", "ID", workSpace.UserId);
            return View(workSpace);
        }

        // POST: WorkSpaces/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("WorkSpaceId,WorkSpaceTitle,WorkSpaceDescription,Status,UserId")] WorkSpace workSpace)
        {
            if (id != workSpace.WorkSpaceId)
            {
                return NotFound();
            }

            if (true)
            {
                try
                {
                    _context.Update(workSpace);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WorkSpaceExists(workSpace.WorkSpaceId))
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
            ViewData["UserId"] = new SelectList(_context.User, "ID", "ID", workSpace.UserId);
            return View(workSpace);
        }

        // GET: WorkSpaces/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {

            ViewBag.WorkSpaceId = id;
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
                .OrderBy(i => i.WorkSpaceId)
                .ToListAsync();

            return View(viewModel);

        }

        // POST: WorkSpaces/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.WorkSpace == null)
            {
                return Problem("Entity set 'TaskHubContext.WorkSpace'  is null.");
            }
            var workSpace = await _context.WorkSpace.FindAsync(id);
            if (workSpace != null)
            {
                _context.WorkSpace.Remove(workSpace);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WorkSpaceExists(int id)
        {
          return (_context.WorkSpace?.Any(e => e.WorkSpaceId == id)).GetValueOrDefault();
        }
    }
}
