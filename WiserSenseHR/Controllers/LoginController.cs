using Microsoft.AspNetCore.Mvc;
using WiserSenseHR.Data;
using WiserSenseHR.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.Data;
using System.Security.Claims;
using System.Configuration;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Microsoft.AspNetCore.Authorization;

namespace WiserSenseHR.Controllers
{
    [AllowAnonymous]
    public class LoginController : Controller
    {
        private readonly WiserDbContext _context;
        private readonly IConfiguration _configuration;

        public LoginController(WiserDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(User loginUser)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginUser.Email && u.Active);

                if (user != null && VerifyPasswordWithSalt(loginUser.Password, user.Password))
                {
                    var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.UserData, user.Name),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role.ToString()) 
            };

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);

                    // Cookie ile giriş yapma
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties
                    {
                        ExpiresUtc = DateTime.UtcNow.AddDays(1),
                        IsPersistent = false 
                    });

                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "Geçersiz e-posta veya şifre.");
            }

            return View(loginUser);
        }

        private bool VerifyPasswordWithSalt(string inputPassword, string storedHashWithSalt)
        {
            if (string.IsNullOrEmpty(storedHashWithSalt) || !storedHashWithSalt.Contains(":"))
                return false;

            var parts = storedHashWithSalt.Split(':');
            if (parts.Length != 2)
                return false;

            var storedHash = parts[0];
            var storedSalt = parts[1];

            var combined = storedSalt + inputPassword;

            using var sha256 = SHA256.Create();
            var inputHash = Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(combined)));

            return storedHash == inputHash;
        }

        //LOGOUT
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Login");
        }


    }

}
