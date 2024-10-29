using Microsoft.AspNetCore.Mvc;
namespace ResumeBuilder.Controllers
{
    public class RegisterController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(string email, string password)
        {
            if (email == "admin@admin.com" && password == "admin")
            { 
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["ErrorMessage"] = "Invalid email or password.";
                return RedirectToAction("Register", "Home");
            } 
        }
    }
}