using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskHub.Data;
using TaskHub.Models.Authentication;
using TaskHub.Models.WorkSpaceViewModels;

namespace TaskHub.Controllers
{
    [Authentication]
    public class DashBoardController : Controller
    {
        private readonly TaskHubContext _context;

        public DashBoardController(TaskHubContext context)
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

        // GET: DashBoard
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
            viewModel.Users = await _context.User
                .Include(i => i.WorkSpaceMembers)
                    .ThenInclude(i => i.WorkSpace)
                .Include(i => i.WorkSpaces)
                .AsNoTracking()
                .OrderBy(i => i.LastName)
                .ToListAsync();

            return View(viewModel);
        }
    }
}
