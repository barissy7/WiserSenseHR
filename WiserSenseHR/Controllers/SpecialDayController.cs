using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WiserSenseHR.Data.Entities;
using WiserSenseHR.Models;

namespace WiserSenseHR.Controllers
{
    [Authorize]
    public class SpecialDayController : Controller
    {
        private readonly WiserDbContext _context;

        public SpecialDayController(WiserDbContext context)
        {
            _context = context;
        }

        // GET: Birthdays
        public async Task<IActionResult> Birthdays()
        {
            var users = await _context.Users.Include(u => u.UserInfo).ToListAsync();

            var today = DateTime.Today.DayOfYear;

            var specialDays = users
                .Select(u =>
                {
                    DateTime? bday = u.UserInfo?.Birthday;
                    int? daysUntil = bday.HasValue
                        ? ((bday.Value.DayOfYear - today + 365) % 365)
                        : (int?)null;

                    return new SpecialDayViewModel
                    {
                        Id = u.Id,
                        Name = u.Name,
                        Birthday = bday,
                        DaysUntilBirthday = daysUntil
                    };
                })
                .OrderBy(vm => vm.DaysUntilBirthday.HasValue ? 0 : 1)
                .ThenBy(vm => vm.DaysUntilBirthday)
                .ToList();

            return View(specialDays);
        }
        
        //GET: JobStarts
        public async Task<IActionResult> JobStarts()
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            IQueryable<User> query = _context.Users
                                             .Include(u => u.UserInfo)
                                             .Where(u => u.UserInfo != null);

            if (User.IsInRole("Admin"))
            {
            }
            else if (User.IsInRole("Viewer") && Guid.TryParse(userId, out var me))
            {
                query = query.Where(u => u.Id == me);
            }
            else
            {
                return Unauthorized();
            }

            var users = await query.ToListAsync();
            var today = DateTime.Today.DayOfYear;

            var model = users
                .Select(u =>
                {
                    DateTime? js = u.UserInfo.JobStartDate;
                    int? daysUntilJs = js.HasValue
                        ? ((js.Value.DayOfYear - today + 365) % 365)
                        : (int?)null;

                    return new SpecialDayViewModel
                    {
                        Id = u.Id,
                        Name = u.Name,
                        Birthday = null,
                        DaysUntilBirthday = null,
                        JobStartDate = js,
                        DaysUntilJobStart = daysUntilJs
                    };
                })
                .OrderBy(vm => vm.DaysUntilJobStart.HasValue ? 0 : 1)
                .ThenBy(vm => vm.DaysUntilJobStart)
                .ToList();

            return View("JobStarts", model);
        }

        // GET: SpecialDay/Details
        public async Task<IActionResult> Details(Guid id)
        {
            var user = await _context.Users.Include(x => x.UserInfo).FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

    }
}


