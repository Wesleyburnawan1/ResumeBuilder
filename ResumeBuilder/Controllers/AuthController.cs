using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using ResumeBuilder.Data;
using ResumeBuilder.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;

using System.Security.Claims;
using System.ComponentModel.DataAnnotations;

namespace ResumeBuilder.Controllers
{
    public class AuthController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AuthController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Account/Register
        public IActionResult Register()
        {
            return View();
        }

        public class RegisterModel
        {
            public string Email { get; set; }
            [Required]
            [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters long.")]
            [RegularExpression(@"^(?=.[a-z])(?=.[A-Z])(?=.\d)(?=.\W).+$", ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter,one number and one special character.")]
            public string Password { get; set; }

            public RegisterModel(string email, string password)
            {
                Email = email;
                Password = password;
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                // Check if the email already exists
                var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
                if (existingUser != null)
                {
                    TempData["ErrorMessage"] = "Email already Registered";
                    return RedirectToAction("Register", "Auth");
                }

                // Hash the password
                var passwordHasher = new PasswordHasher<User>();
                var hashedPassword = passwordHasher.HashPassword(null, model.Password);

                // Create a new user
                var newUser = new User
                {
                    Email = model.Email,
                    PasswordHash = hashedPassword
                };

                // Save the new user to the database
                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();

                // Optionally, you can redirect to the login page or a confirmation page
                return RedirectToAction("Login");
            }

            return View(model);
        }
        public class LoginModel
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }


        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
                if (user != null)
                {
                    var passwordHasher = new PasswordHasher<User>();
                    var verificationResult = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, model.Password);

                    if (verificationResult == PasswordVerificationResult.Success)
                    {
                        HttpContext.Session.SetString("Email", user.Email);

                        return RedirectToAction("Index", "Home", new { email = user.Email });
                    }
                }

                // If we reach here, login failed
                TempData["ErrorMessage"] = "Invalid email or password";
                return RedirectToAction("Login", "Auth");
            }

            return View(model);
        }
        // Logout action (if you add cookie authentication)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            // Sign the user out
            HttpContext.Session.Clear();

            await HttpContext.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }
    }
}