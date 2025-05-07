using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WiserSenseHR.Models;

namespace WiserSenseHR.Controllers
{
    [Authorize(Roles = "Admin, Viewer, Editor")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly WiserDbContext _context;

        public HomeController(ILogger<HomeController> logger, WiserDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var userCount = await _context.Users.CountAsync(u => u.Active);
            var meetingCount = await _context.Meetings.CountAsync();

            ViewData["UserCount"] = userCount;
            ViewData["MeetingCount"] = meetingCount;

            return View();
        }


        [HttpGet]
        public async Task<IActionResult> GetMeetings()
        {
            var meetings = await _context.Meetings
                                         //.Where(m => m.MeetingDate >= DateTime.Now) 
                                         .ToListAsync();

            var events = meetings.Select(m => new
            {
                title = m.Name,
                start = m.MeetingDate.ToString("yyyy-MM-dd'T'HH:mm:ss"), 
                end = m.EndDate.ToString("yyyy-MM-dd'T'HH:mm:ss"),
                description = m.Description
            });

            return Json(events);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
