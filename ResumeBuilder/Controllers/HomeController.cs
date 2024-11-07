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

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Register()
    {
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
