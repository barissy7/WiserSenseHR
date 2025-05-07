using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WiserSenseHR.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using WiserSenseHR.Data.Entities;
using FluentValidation;
using FluentValidation.Results;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Text;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace WiserSenseHR.Controllers
{
    [Authorize]
    public class UserController : Controller
    {

        private readonly WiserDbContext _context;
        private readonly IValidator<User> _validator;
        private readonly IValidator<UserInfo> _userInfoValidator;

        public UserController(WiserDbContext context, IValidator<User> validator, IValidator<UserInfo> userInfoValidator)
        {
            _context = context;
            _validator = validator;
            _userInfoValidator = userInfoValidator;
        }
        // GET: UserController
        public async Task<IActionResult> Index()
        {
            // Admin ise tüm aktif kullanıcıları getir
            if (User.IsInRole("Admin"))
            {
                var users = await _context.Users
                                          .Where(x => x.Active)
                                          .ToListAsync();
                return View(users);
            }

            // Viewer ise sadece kendi bilgisini 
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (Guid.TryParse(userIdClaim, out var userId))
            {
                var singleUser = await _context.Users
                                               .Where(x => x.Id == userId && x.Active)
                                               .ToListAsync();
                return View(singleUser);
            }

            // Claim bulunamazsa erişim reddi
            return Forbid();
        }

        // GET: User/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: User/Create
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(User user)
        {
            user.Active = true;
            ValidationResult validationResult = await _validator.ValidateAsync(user);

            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
                return View(user);
            }
            // Şifreyi hashleme
            user.Password = HashPasswordWithSalt(user.Password);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        private string HashPasswordWithSalt(string password)
        {
            var saltBytes = new byte[16];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(saltBytes);

            var salt = Convert.ToBase64String(saltBytes);
            var combined = salt + password;

            using var sha256 = SHA256.Create();
            var hash = Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(combined)));

            return $"{hash}:{salt}";
        }



        // GET: User/Edit
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var user = await _context.Users
                                      .Include(u => u.UserInfo) 
                                      .FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: User/Edit
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            var existingUser = await _context.Users.Include(u => u.UserInfo).FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
                if (existingUser == null)
            {
                return NotFound();
            }

            if (existingUser.UserInfo == null)
            {
                return NotFound();
            }

            ValidationResult validationResult = await _validator.ValidateAsync(user);

            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
                return View(user);
            }

            existingUser.Name = user.Name;
            existingUser.Email = user.Email;
            //existingUser.Password = user.Password;
            existingUser.Role = user.Role;
            existingUser.Active = user.Active;
            existingUser.Department = user.Department;
            existingUser.UserInfo.JobTitle = user.UserInfo.JobTitle;
            existingUser.UserInfo.PhoneNumber = user.UserInfo.PhoneNumber;
            existingUser.UserInfo.Birthday = user.UserInfo.Birthday;
            existingUser.UserInfo.JobStartDate = user.UserInfo.JobStartDate;

            //Güncelleme sırasında admin şifreyi boş bırkaırsa mevcut şifre korunur
            if (!string.IsNullOrWhiteSpace(user.Password))
            {
                existingUser.Password = HashPasswordWithSalt(user.Password);
            }

            try
            {
                _context.Update(existingUser);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                ModelState.AddModelError("", "Güncelleme sırasında hata oluştu.");
                return View(existingUser);
            }

            return RedirectToAction("Index");
        }

        // GET: User/Delete
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: User/Delete
        [Authorize(Roles = "Admin")]

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SoftDelete(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            try
            {
                user.Active = false; // Kullanıcıyı pasif hale getirme
                _context.Update(user);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                ModelState.AddModelError("", "Kullanıcı silme işlemi başarısız oldu.");
                return View(user);
            }

            return RedirectToAction("Index");
        }

        // POST: User/HardDelete
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> HardDelete(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            try
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Kullanıcı kalıcı olarak silinemedi.");
                return View("InactiveUsers"); 
            }

            return RedirectToAction("InactiveUsers");
        }

        // GET: User/InactiveUsers
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> InactiveUsers(Guid id)
        {
            var inactiveUsers = await _context.Users
                .Where(x => x.Active == false)
                .ToListAsync();

            return View(inactiveUsers);
        }

        // GET: User/InactiveUsers
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ActivateUser(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            try
            {
                user.Active = true; // Kullanıcıyı tekrar aktif yap
                _context.Update(user);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                ModelState.AddModelError("", "Kullanıcıyı aktifleştirme işlemi başarısız oldu.");
                return RedirectToAction("InactiveUsers");
            }

            return RedirectToAction("Index");
        }

        // GET: User/Details/
        [Authorize]
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
