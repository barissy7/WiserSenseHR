using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WiserSenseHR.Data.Entities;

namespace WiserSenseHR.Controllers
{
    [Authorize]
    public class FoodListController : Controller
    {
        private readonly WiserDbContext _context;
        private readonly IValidator<FoodList> _validator;

        public FoodListController(WiserDbContext context, IValidator<FoodList> validator)
        {
            _context = context;
            _validator = validator;
        }

        public async Task<IActionResult> Index()
        {
            var foods = await _context.FoodLists.ToListAsync();
            return View(foods);
        }

        // GET: FoodList/Create
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: FoodList/Create
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FoodList foodList, IFormFile newImage)
        {
            ValidationResult validationResult = await _validator.ValidateAsync(foodList);

            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }

                if (newImage == null || newImage.Length == 0)
                {
                    ModelState.AddModelError("ImageUrl", "Lütfen bir resim dosyası seçin.");
                }

                return View(foodList); 
            }

            if (newImage != null && newImage.Length > 0)
            {
                var fileExtension = Path.GetExtension(newImage.FileName).ToLower();

                if (fileExtension != ".jpg" && fileExtension != ".jpeg" && fileExtension != ".png")
                {
                    ModelState.AddModelError("ImageUrl", "Yalnızca .jpg, .jpeg veya .png formatındaki dosyalar yüklenebilir.");
                    return View(foodList);
                }

                var newImagePath = Path.Combine("images", Path.GetFileName(newImage.FileName));
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", newImagePath);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await newImage.CopyToAsync(stream);
                }

                foodList.ImageUrl = Path.GetFileName(newImage.FileName); 
            }

            _context.Add(foodList);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index"); 
        }


        // GET: FoodList/Delete
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
                return NotFound();

            var food = await _context.FoodLists
                .FirstOrDefaultAsync(f => f.Id == id.Value);

            if (food == null)
                return NotFound();

            return View(food);
        }

        // POST: FoodList/Delete
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var food = await _context.FoodLists.FindAsync(id);
            if (food != null)
            {
                var fullImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", food.ImageUrl);
                if (System.IO.File.Exists(fullImagePath))
                {
                    System.IO.File.Delete(fullImagePath);
                }

                _context.FoodLists.Remove(food);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        // GET: FoodList/Edit
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var food = await _context.FoodLists.FindAsync(id);
            if (food == null)
            {
                return NotFound();
            }

            return View(food);
        }

        // POST: FoodList/Edit
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, FoodList foodList, IFormFile newImage)
        {
            if (id != foodList.Id)
                return BadRequest();

            // 1) Model doğrulama
            var result = await _validator.ValidateAsync(foodList);
            if (!result.IsValid)
            {
                foreach (var e in result.Errors)
                    ModelState.AddModelError(e.PropertyName, e.ErrorMessage);
                return View(foodList);
            }

            var food = await _context.FoodLists.FindAsync(id);
            if (food == null) 
                return NotFound();

            food.Description = foodList.Description;

            if (newImage != null && newImage.Length > 0)
            {
                var ext = Path.GetExtension(newImage.FileName).ToLower();
                if (new[] { ".jpg", ".jpeg", ".png" }.Contains(ext))
                {
                    var imagesDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
                    if (!Directory.Exists(imagesDir))
                        Directory.CreateDirectory(imagesDir);

                    var oldFile = Path.Combine(imagesDir, food.ImageUrl);
                    if (System.IO.File.Exists(oldFile))
                        System.IO.File.Delete(oldFile);

                    var fileName = Path.GetFileName(newImage.FileName);
                    var filePath = Path.Combine(imagesDir, fileName);
                    using var stream = new FileStream(filePath, FileMode.Create);
                    await newImage.CopyToAsync(stream);

                    food.ImageUrl = fileName;
                }
                else
                {
                    ModelState.AddModelError("ImageUrl", "Yalnızca .jpg/.jpeg/.png formatı desteklenir.");
                    return View(food);
                }
            }

            _context.Update(food);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}




