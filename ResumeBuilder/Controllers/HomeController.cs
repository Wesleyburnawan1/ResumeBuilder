using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ResumeBuilder.Data;
using ResumeBuilder.Models;
using QRCoder;
using static QRCoder.PayloadGenerator;
using System.Drawing;

namespace ResumeBuilder.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _context;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpGet]
    public IActionResult Delete(string entityType, int id)
    {
        object entity = null;

        switch (entityType.ToLower())
        {
            case "education":
                entity = _context.Education.FirstOrDefault(e => e.EducationID == id);
                break;
            case "project":
                entity = _context.Projects.FirstOrDefault(p => p.ProjectID == id);
                break;
            case "certification":
                entity = _context.Certifications.FirstOrDefault(c => c.CertificationID == id);
                break;

            case "workexperience":
                entity = _context.WorkExperience.FirstOrDefault(W => W.WorkExperienceID == id);
                break;

            case "skills":
                entity = _context.Skills.FirstOrDefault(c => c.SkillID == id);
                break;
            // Add more cases for other content types
            default:
                return NotFound("Entity type not recognized.");
        }

        if (entity == null)
        {
            return NotFound();
        }

        // Remove the entity from the correct DbSet based on the entityType
        switch (entityType.ToLower())
        {
            case "education":
                _context.Education.Remove((Education)entity);
                break;
            case "project":
                _context.Projects.Remove((Projects)entity);
                break;
            case "certification":
                _context.Certifications.Remove((Certifications)entity);
                break;
            case "workexperience":
                _context.WorkExperience.Remove((WorkExperience)entity);
                break;
            case "skills":
                _context.Skills.Remove((Skills)entity);
                break;

        }
        _context.SaveChanges();
        return RedirectToAction("Index"); // Or to any other page that shows the list of items
    }

    public IActionResult Index(string email)
    {
        string userEmail = HttpContext.Session.GetString("Email");
        int userid = (int)HttpContext.Session.GetInt32("UserID");

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
        ViewBag.UserId = userid; // Pass userId to the view

        return View();
    }
    [HttpGet]
    public IActionResult GenerateQRCode()
    {
        try
        {
            int userid = (int)HttpContext.Session.GetInt32("UserID");
            string email = HttpContext.Session.GetString("Email"); // Retrieve email from session
            string QRString = Url.Action("ResumeView", "Home", new { UserID = userid }, Request.Scheme);

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
    public async Task<IActionResult> ResumeView()
    {
        if (Request.Query.ContainsKey("userId"))
        {
            int userId;
            bool isValidUserId = int.TryParse(Request.Query["userId"], out userId);

            if (isValidUserId)
            {
                // Fetch the user based on the UserId from the database
                var user = await _context.UserDetails
                                         .FirstOrDefaultAsync(u => u.UserID == userId);

                if (user == null)
                {
                    return NotFound("User not found");
                }

                // Fetch related data using the UserId
                var resumeViewModel = new ResumeViewModel
                {
                    User = user,
                    EducationList = await _context.Education
                                                   .Where(e => e.UserID == user.UserID)
                                                   .ToListAsync(),
                    SkillsList = await _context.Skills
                                               .Where(s => s.UserID == user.UserID)
                                               .ToListAsync(),
                    WorkExperienceList = await _context.WorkExperience
                                                       .Where(w => w.UserID == user.UserID)
                                                       .ToListAsync(),
                    ProjectsList = await _context.Projects
                                                 .Where(p => p.UserID == user.UserID)
                                                 .ToListAsync(),
                    CertificationsList = await _context.Certifications
                                                       .Where(c => c.UserID == user.UserID)
                                                       .ToListAsync()
                };

                return View(resumeViewModel);
            }
            else
            {
                // If the UserId is invalid or not a valid integer, return an error
                return BadRequest("Invalid UserId parameter");
            }
        }
        else
        {
            // If UserId is not found in the query string, return an error or redirect
            return BadRequest("UserId parameter is missing");
        }
    }
    public IActionResult Certifications()
    {
        int UserID = (int)HttpContext.Session.GetInt32("UserID"); // Assuming Email is stored in session


        var certificationslist = _context.Certifications.Where(e => e.UserID == UserID) // Replace with the correct property name
.ToList();
        return View(certificationslist ?? new List<Certifications>()); // Ensure a valid list is passed

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
        int UserID = (int)HttpContext.Session.GetInt32("UserID");
        var educationList = _context.Education.Where(e => e.UserID == UserID)
.ToList();
        return View(educationList ?? new List<Education>());
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
        int? userId = HttpContext.Session.GetInt32("UserID");

        if (userId == null)
        {
            // Handle the case where UserID is not available in session (e.g., redirect to login page)
            return RedirectToAction("Login", "Account"); // Example, adjust as per your login flow
        }

        // Retrieve the user's details from the database using the UserID from the session
        var userDetails = _context.UserDetails.FirstOrDefault(u => u.UserID == userId);

        if (userDetails == null)
        {
            userDetails = new UserDetails(); // If no data found, create a new empty model
        }
        ViewData["Email"] = HttpContext.Session.GetString("Email");

        return View(userDetails);

    }
    [HttpPost]
    public IActionResult SubmitPersonalDetails(UserDetails userDetails)
    {
        // Retrieve UserID from session
        int? userId = HttpContext.Session.GetInt32("UserID");

        if (userId == null)
        {
            // Redirect to login if UserID is not in session
            return RedirectToAction("Login", "Account");
        }

        if (ModelState.IsValid)
        {
            var existingUser = _context.UserDetails.FirstOrDefault(u => u.UserID == userId);
            if (existingUser != null)
            {
                // Update existing user details
                existingUser.FirstName = userDetails.FirstName;
                existingUser.LastName = userDetails.LastName;
                existingUser.Address = userDetails.Address;
            }
            else
            {
                userDetails.UserID = userId.Value; // Ensure UserID is set
                _context.UserDetails.Add(userDetails);
            }

            _context.SaveChanges(); // Save changes to the database
            return RedirectToAction("PersonalDetails"); // Redirect back to PersonalDetails page
        }

        // If model state is invalid (e.g., validation errors), return the form with the user details for correction
        return View(userDetails);
    }

    public IActionResult Projects()
    {
        int UserID = (int)HttpContext.Session.GetInt32("UserID"); // Assuming Email is stored in session


        var projectlist = _context.Projects.Where(e => e.UserID == UserID) // Replace with the correct property name
.ToList();
        return View(projectlist ?? new List<Projects>()); // Ensure a valid list is passed
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
        int UserID = (int)HttpContext.Session.GetInt32("UserID"); // Assuming Email is stored in session


        var skilllist = _context.Skills.Where(e => e.UserID == UserID) // Replace with the correct property name
.ToList();
        return View(skilllist ?? new List<Skills>()); // Ensure a valid list is passed

    }
    [HttpPost]
    public async Task<IActionResult> SubmitSkills(Skills model)
    {
        if (ModelState.IsValid)
        {
            int? userID = HttpContext.Session.GetInt32("UserID");
            model.UserID = userID.Value;
            _context.Skills.Add(model);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");  // Redirect to Home or other success page
        }
        return View("Index");
    }
    public IActionResult WorkExperience()
    {
        int UserID = (int)HttpContext.Session.GetInt32("UserID"); // Assuming Email is stored in session


        var WorkExperienceList = _context.WorkExperience.Where(e => e.UserID == UserID) // Replace with the correct property name
.ToList();
        return View(WorkExperienceList ?? new List<WorkExperience>()); // Ensure a valid list is passed
    }
    [HttpPost]
    public async Task<IActionResult> SubmitWorkExperience(WorkExperience model)
    {
        if (ModelState.IsValid)
        {

            int? userID = HttpContext.Session.GetInt32("UserID");
            model.UserID = userID.Value;
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
