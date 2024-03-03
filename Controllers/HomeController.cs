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
                .Where(i => i.WorkSpaceMembers.Any(wm => wm.User.ID == 2))
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
