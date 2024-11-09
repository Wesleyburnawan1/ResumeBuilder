using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ResumeBuilder.Models;

namespace ResumeBuilder.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index(string email)
    {
        // First, check if the email exists in the session
        string userEmail = HttpContext.Session.GetString("Email");

        // If no email found in session and query string is empty, redirect to login page
        if (string.IsNullOrEmpty(userEmail) && string.IsNullOrEmpty(email))
        {
            return RedirectToAction("Login", "Account");
        }

        // If the email is found in the session, use it; otherwise, fallback to the query string
        if (string.IsNullOrEmpty(userEmail) && !string.IsNullOrEmpty(email))
        {
            userEmail = email; // Take email from the query string
        }

        // You can pass the email to the view or use it for other purposes
        ViewBag.UserEmail = userEmail;

        return View();
    }

    
    public IActionResult Privacy()
    {
        return View();
    }
    public IActionResult Certifications()
    {
        return View();
    }
    public IActionResult Education()
    {
        return View();
    }
    public IActionResult PersonalDetails()
    {
        return View();
    }
    public IActionResult Projects()
    {
        return View();
    }

    public IActionResult Login()
    {
        return View();
    }
    public IActionResult Skills()
    {
        return View();
    }
    public IActionResult WorkExperience()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
