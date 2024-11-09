using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ResumeBuilder.Controllers;
using ResumeBuilder.Data;
using ResumeBuilder.Models;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace ResumeBuilder.Tests
{
    public class AuthControllerTests
    {
        private DbContextOptions<ApplicationDbContext> CreateNewContextOptions()
        {
            return new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
        }

        private void ClearDatabase(ApplicationDbContext context)
        {
            context.Users.RemoveRange(context.Users);
            context.SaveChanges();
        }

        [Fact]
        public async Task Register_Post_ReturnsRedirectToActionResult_WhenModelStateIsValid()
        {
            // Arrange
            var options = CreateNewContextOptions();

            using var context = new ApplicationDbContext(options);
            ClearDatabase(context);
            var controller = new AuthController(context);

            var tempData = new Mock<ITempDataDictionary>();
            controller.TempData = tempData.Object;

            var model = new AuthController.RegisterModel("test@example.com", "Password123!");

            // Act
            var result = await controller.Register(model);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Login", redirectToActionResult.ActionName);
        }

        [Fact]
        public async Task Register_Post_ReturnsViewResult_WhenModelStateIsInvalid()
        {
            // Arrange
            var options = CreateNewContextOptions();

            using var context = new ApplicationDbContext(options);
            ClearDatabase(context);
            var controller = new AuthController(context);
            controller.ModelState.AddModelError("Email", "Required");

            var tempData = new Mock<ITempDataDictionary>();
            controller.TempData = tempData.Object;

            var model = new AuthController.RegisterModel("test@example.com", "Password123!");

            // Act
            var result = await controller.Register(model);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(model, viewResult.Model);
        }

        [Fact]
        public async Task Register_Post_ReturnsRedirectToActionResult_WhenEmailAlreadyExists()
        {
            // Arrange
            var options = CreateNewContextOptions();

            using var context = new ApplicationDbContext(options);
            ClearDatabase(context);
            context.Users.Add(new User { Email = "test@example.com", PasswordHash = "hashedpassword" });
            await context.SaveChangesAsync();

            var controller = new AuthController(context);

            var tempData = new Mock<ITempDataDictionary>();
            controller.TempData = tempData.Object;

            var model = new AuthController.RegisterModel("test@example.com", "Password123!");

            // Act
            var result = await controller.Register(model);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Register", redirectToActionResult.ActionName);
            Assert.Equal("Auth", redirectToActionResult.ControllerName);
        }
    }
}