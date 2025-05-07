using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WiserSenseHR.Data.Entities;

namespace WiserSenseHR.Controllers
{
    [Authorize]
    public class RuleController : Controller
    {

        private readonly WiserDbContext _context;
        private readonly IValidator<Rule> _validator;

        public RuleController(WiserDbContext context, IValidator<Rule> validator)
        {
            _context = context;
            _validator = validator;
        }
        public async Task<IActionResult> Index()
        {
            var rules = await _context.Rules.ToListAsync();
            return View(rules);
        }

        // GET: Rule/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Rule/Create
        [Authorize(Roles = "Admin")]
        [HttpPost] 
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Rule model)
        {
            ValidationResult result = await _validator.ValidateAsync(model);
            if (!result.IsValid)
            {
                foreach (var e in result.Errors)
                    ModelState.AddModelError(e.PropertyName, e.ErrorMessage);
                return View(model);
            }

            _context.Rules.Add(model);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // GET: Rule/Edit
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var rule = await _context.Rules.FindAsync(id);
            if (rule == null) return NotFound();
            return View(rule);
        }

        // POST: Rule/Edit
        [Authorize(Roles = "Admin")]
        [HttpPost] 
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Rule model)
        {
            if (id != model.Id)
                return BadRequest();

            ValidationResult result = await _validator.ValidateAsync(model);
            if (!result.IsValid)
            {
                foreach (var e in result.Errors)
                    ModelState.AddModelError(e.PropertyName, e.ErrorMessage);
                return View(model);
            }

            var existing = await _context.Rules.FindAsync(id);
            if (existing == null) return NotFound();

            existing.Description = model.Description;

            try
            {
                _context.Update(existing);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                ModelState.AddModelError("", "Güncelleme sırasında bir hata oluştu.");
                return View(model);
            }

            return RedirectToAction("Index");
        }
    }
}

