using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WiserSenseHR.Data.Entities;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;

namespace WiserSenseHR.Controllers
{
    [Authorize]
    public class AnnouncementController : Controller
    {
        private readonly WiserDbContext _context;
        private readonly IValidator<Announcement> _validator;

        public AnnouncementController(WiserDbContext context, IValidator<Announcement> validator)
        {
            _context = context;
            _validator = validator;
        }

        // GET: Announcement/Index
        public async Task<IActionResult> Index()
        {
            var announcements = await _context.Announcements.ToListAsync();
            return View(announcements);
        }

        // GET: Announcement/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Announcement/Create
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Announcement announcement)
        {
            ValidationResult validationResult = await _validator.ValidateAsync(announcement);

            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
                return View(announcement);
            }

            _context.Announcements.Add(announcement);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        // GET: Announcement/Edit/
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var announcement = await _context.Announcements.FindAsync(id);
            if (announcement == null)
            {
                return NotFound();
            }

            return View(announcement);
        }

        // POST: Announcement/Edit/
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Announcement announcement)
        {
            if (id != announcement.Id)
            {
                return BadRequest();
            }

            var existingAnnouncement = await _context.Announcements.FindAsync(id);
            if (existingAnnouncement == null)
            {
                return NotFound();
            }

            ValidationResult validationResult = await _validator.ValidateAsync(announcement);

            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
                return View(announcement);
            }

            existingAnnouncement.Name = announcement.Name;
            existingAnnouncement.Description = announcement.Description;
            existingAnnouncement.AnnouncementDate = announcement.AnnouncementDate;

            try
            {
                _context.Update(existingAnnouncement);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                ModelState.AddModelError("", "Duyuru güncellenirken bir hata oluştu.");
                return View(existingAnnouncement);
            }

            return RedirectToAction("Index");
        }
    }
}
