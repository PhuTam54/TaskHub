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

        // GET: Home/MyBoard/1
        public async Task<IActionResult> MyBoard(int id = 1)
        {
            ViewBag.BoardId = id;
            var memberId = 2;
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
                .Where(i => i.Boards.Any(b => b.BoardId == id))
                .ToListAsync();

            ViewBag.DataFromDatabase = viewModel.WorkSpaces;
            return View(viewModel);
        }
    }
}
