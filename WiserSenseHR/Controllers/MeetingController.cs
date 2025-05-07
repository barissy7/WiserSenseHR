using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WiserSenseHR.Data.Entities;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;

namespace WiserSenseHR.Controllers
{
    [Authorize]
    public class MeetingController : Controller
    {
        private readonly WiserDbContext _context;
        private readonly IValidator<Meeting> _validator;

        public MeetingController(WiserDbContext context, IValidator<Meeting> validator)
        {
            _context = context;
            _validator = validator;
        }

        //GET MeetingController
        public async Task<IActionResult> Index()
        {
            var meetings = await _context.Meetings.ToListAsync();
            return View(meetings);
        }

        // GET: Meeting/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Meeting/Create
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Meeting meeting)
        {
            ValidationResult validationResult = await _validator.ValidateAsync(meeting);

            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
                return View(meeting);
            }

            _context.Meetings.Add(meeting);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        // GET: Meeting/Edit
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var meeting = await _context.Meetings.FindAsync(id);
            if (meeting == null)
            {
                return NotFound();
            }

            return View(meeting);
        }

        // POST: Meeting/Edit/
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Meeting meeting)
        {
            if (id != meeting.Id)
            {
                return BadRequest();
            }

            var existingMeeting = await _context.Meetings.FindAsync(id);
            if (existingMeeting == null)
            {
                return NotFound();
            }

            ValidationResult validationResult = await _validator.ValidateAsync(meeting);

            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
                return View(meeting);
            }

            existingMeeting.Name = meeting.Name;
            existingMeeting.Description = meeting.Description;
            existingMeeting.MeetingDate = meeting.MeetingDate;

            try
            {
                _context.Update(existingMeeting);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                ModelState.AddModelError("", "Toplantı güncellenirken bir hata oluştu.");
                return View(existingMeeting);
            }

            return RedirectToAction("Index");
        }

        // GET: Meeting/Delete
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var meeting = await _context.Meetings.FindAsync(id);
            if (meeting == null)
            {
                return NotFound();
            }

            return View(meeting); 
        }

        // POST: Meeting/Delete
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id, Meeting meeting)
        {
            var existingMeeting = await _context.Meetings.FindAsync(id);
            if (existingMeeting == null)
            {
                return NotFound();
            }

            _context.Meetings.Remove(existingMeeting);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

    }
}
