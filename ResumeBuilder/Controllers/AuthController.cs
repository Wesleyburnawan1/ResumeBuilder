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
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        public class RegisterModel
        {
            public string Email { get; set; }
            [Required]
            [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters long.")]
            [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*\W).+$", ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter,one number and one special character.")]
            public string Password { get; set; }

            public RegisterModel(string email, string password) {
                Email = email;
                Password = password;    
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (model.Email == "admin@admin.com" && model.Password == "admin")
            { 
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["ErrorMessage"] = "Invalid email or password.";
                return RedirectToAction("Register", "Auth");
            } 
        }
        public class LoginModel
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (model.Email == "admin@admin.com" && model.Password == "admin")
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["ErrorMessage"] = "Invalid email or password.";
                return RedirectToAction("Login", "Auth");
            }
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