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

        [HttpPost]
        public List<object> GetToTalUsers()
        {
            List<object> data = new List<object>();
            List<string> labels = _context.User.Select(m => m.UserName).ToList();
            List<int> total = _context.User.Select(t => t.ID).ToList();
            data.Add(labels);
            data.Add(total);
            return data;
        }
        [HttpPost]
        public List<object> GetToTal()
        {
            List<object> data = new List<object>();

            int totalUser = _context.User.Count();
            int totalWorkSpaces = _context.WorkSpace.Count();
            int totalWorkSpaceMembers = _context.WorkSpaceMember.Count();
            data.Add(totalUser);
            data.Add(totalWorkSpaces);
            data.Add(totalWorkSpaceMembers);

            return data;
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
            HttpContext.Session.SetInt32("WorkSpaceID", id);
            var userId = HttpContext.Session.GetInt32("UserID");
            var userName = HttpContext.Session.GetString("UserName");
            var userAvatar = HttpContext.Session.GetString("Avatar");

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
        public async Task<IActionResult> JoinWorkSpace([Bind("MemberId,WorkSpaceId,UserId,EnrollmentDate,Status")] WorkSpaceMember workSpaceMember)
        {
            _context.Add(workSpaceMember);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
