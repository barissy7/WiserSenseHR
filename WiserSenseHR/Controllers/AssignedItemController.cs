using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WiserSenseHR.Data.Entities;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.Rendering;
using WiserSenseHR.Data.Enums;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace WiserSenseHR.Controllers
{
    [Authorize]
    public class AssignedItemController : Controller
    {
        private readonly WiserDbContext _context;
        private readonly IValidator<AssignedItem> _validator;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AssignedItemController(WiserDbContext context, IValidator<AssignedItem> validator, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _validator = validator;
            _httpContextAccessor = httpContextAccessor;
        }

        // GET: Assigned/Index
        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (User.IsInRole("Admin"))
            {
                // Admine tüm eşyalar gösterilir
                var allAssignedItems = await _context.AssignedItems.Include(ai => ai.User).ToListAsync();
                return View(allAssignedItems);
            }

            if (User.IsInRole("Viewer"))
            {
                // Viewera sadece kendi üzerine kayıtlı eşyalar
                if (userId != null)
                {
                    var assignedItems = await _context.AssignedItems
                        .Where(ai => ai.UserId.ToString() == userId) 
                        .Include(ai => ai.User) 
                        .ToListAsync();
                    return View(assignedItems);
                }
            }

            return Unauthorized(); 
        }


        // GET: Assigned/Create
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            ViewBag.Users = await _context.Users
                 .Select(u => new SelectListItem
                 {
                     Value = u.Id.ToString(), 
                     Text = u.Name 
                 })
                  .ToListAsync();
            return View();
        }

        // POST: Assigned/Create
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AssignedItem assignedItem, List<AssignedItemName> ItemNames)
        {

            assignedItem.ItemNames = ItemNames;
            ValidationResult validationResult = await _validator.ValidateAsync(assignedItem);

            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
               
                return View(assignedItem);
            }

            assignedItem.Id = Guid.NewGuid(); 
            _context.AssignedItems.Add(assignedItem);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        // GET: Assigned/Edit
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var assignedItem = await _context.AssignedItems.FindAsync(id);
            if (assignedItem == null)
            {
                return NotFound();
            }

            ViewBag.Users = await _context.Users
                .Select(u => new SelectListItem
                {
                    Value = u.Id.ToString(), 
                    Text = u.Name 
                })
                 .ToListAsync();
            return View(assignedItem);
        }

        // POST: Assigned/Edit
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, AssignedItem assignedItem)
        {
            if (id != assignedItem.Id)
            {
                return BadRequest();
            }

            var existingAssignedItem = await _context.AssignedItems.FindAsync(id);
            if (existingAssignedItem == null)
            {
                return NotFound();
            }

            ValidationResult validationResult = await _validator.ValidateAsync(assignedItem);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
                ViewBag.Users = await _context.Users.ToListAsync();
                return View(assignedItem);
            }

            existingAssignedItem.ItemNames = assignedItem.ItemNames;
            existingAssignedItem.Description = assignedItem.Description;
            existingAssignedItem.UserId = assignedItem.UserId;

            try
            {
                _context.Update(existingAssignedItem);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                ModelState.AddModelError("", "Eşyalar güncellenirken hata oluştu.");
                return View(existingAssignedItem);
            }

            return RedirectToAction("Index");
        }

        // GET: Assigned/Delete
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var assignedItem = await _context.AssignedItems
            .Include(a => a.User) 
            .FirstOrDefaultAsync(m => m.Id == id);

            if (assignedItem == null)
            {
                return NotFound();
            }

            return View(assignedItem);
        }

        // POST: Assigned/Delete
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirm(Guid id)
        {
            var assignedItem = await _context.AssignedItems.FindAsync(id);
            if (assignedItem == null)
            {
                return NotFound();
            }

            _context.AssignedItems.Remove(assignedItem);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}


