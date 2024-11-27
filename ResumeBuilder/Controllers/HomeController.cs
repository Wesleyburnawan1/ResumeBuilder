using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;


using Microsoft.EntityFrameworkCore;
using ResumeBuilder.Data;
using ResumeBuilder.Models;

using ResumeBuilder.Models;
using QRCoder;
using static QRCoder.PayloadGenerator;
using System.Drawing;

namespace ResumeBuilder.Controllers;

public class HomeController : Controller
{private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _context;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }


    public IActionResult Index(string email)
    { 
        string userEmail = HttpContext.Session.GetString("Email"); 
        if (string.IsNullOrEmpty(userEmail) && string.IsNullOrEmpty(email))
        {
            return RedirectToAction("Login", "Account");
        } 
        if (string.IsNullOrEmpty(userEmail) && !string.IsNullOrEmpty(email))
        {
            userEmail = email; 
            TempData["Email"] = email; 
        } 
        ViewBag.UserEmail = userEmail;
        TempData["Email"] = email;
        return View();
    }

    [HttpGet]
    public IActionResult GenerateQRCode()
    {
        try
        { 
            string email = HttpContext.Session.GetString("Email"); // Retrieve email from session
            string QRString = Url.Action("ResumeView", "Home", new { email = email }, Request.Scheme);

            using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
            {
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(QRString, QRCodeGenerator.ECCLevel.Q);
                PngByteQRCode pngByteQRCode = new PngByteQRCode(qrCodeData); 
                byte[] qrCodeImage = pngByteQRCode.GetGraphic(5); 
                return File(qrCodeImage, "image/png");
            }
        }
        catch (Exception ex)
        {
            return Content($"Error generating QR Code: {ex.Message}");
        }
    }
    public IActionResult Privacy()
    {
        return View();
    }
    public IActionResult ResumeView()
    { 
        string sessionemail = HttpContext.Session.GetString("Email");
        string requestemail = Request.Query["email"]; 
        if(requestemail != sessionemail)
        {
           requestemail  = sessionemail; 
            TempData["Email"] = requestemail;
        }
        ViewData["Email"] = sessionemail;
        TempData["Email"] = requestemail;

        return View();
    }
    public IActionResult Certifications()
    {
        return View();
    }
      [HttpPost]
    public async Task<IActionResult> SubmitCertifications(Certifications model)
    {
        if (ModelState.IsValid)
        {

            int? userID = HttpContext.Session.GetInt32("UserID");
            model.UserID = userID.Value;  // Assign the UserID from session to the model

            _context.Certifications.Add(model);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");  // Redirect to Home or other success page
        }

        return View("Index");
    }


    public IActionResult Education()
    {
        return View();
    }
        [HttpPost]
    public async Task<IActionResult> SubmitEducation(Education model)
    {
        if (ModelState.IsValid)
        {

            int? userID = HttpContext.Session.GetInt32("UserID");
            model.UserID = userID.Value;  // Assign the UserID from session to the model

            _context.Education.Add(model);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");  // Redirect to Home or other success page
        }

        return View("Index");
    }

    public IActionResult PersonalDetails()
    {
        return View();
    }
    public IActionResult Projects()
    {
        return View();
    }
 public async Task<IActionResult> SubmitProjects(Projects model)
    {
        if (ModelState.IsValid)
        {

            int? userID = HttpContext.Session.GetInt32("UserID");
            model.UserID = userID.Value;  // Assign the UserID from session to the model

            _context.Projects.Add(model);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");  // Redirect to Home or other success page
        }

        return View("Index");
    }

    public IActionResult Login()
    {
        HttpContext.Session.Clear();

        return View();
    }
    public IActionResult Skills()
    {
        return View();
    }
      [HttpPost]
    public async Task<IActionResult> SubmitSkills(Skills model)
    {
        if (ModelState.IsValid)
        {

            int? userID = HttpContext.Session.GetInt32("UserID");
            model.UserID = userID.Value;  // Assign the UserID from session to the model

            _context.Skills.Add(model);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");  // Redirect to Home or other success page
        }

        return View("Index");
    }

  

    public IActionResult WorkExperience()
    {
        return View();
    }
 [HttpPost]
    public async Task<IActionResult> SubmitWorkExperience(WorkExperience model)
    {
        if (ModelState.IsValid)
        {

            int? userID = HttpContext.Session.GetInt32("UserID");
            model.UserID = userID.Value;  // Assign the UserID from session to the model

            _context.WorkExperience.Add(model);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");  // Redirect to Home or other success page
        }

        return View("Index");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
