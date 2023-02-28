using Car_Rental_MVC.Models;
using Car_Rental_MVC.Repositories;
using Car_Rental_MVC.Repositories.IRepositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Car_Rental_MVC.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public AuthenticationController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("Login")]
        [AllowAnonymous]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return Redirect("/");
            }
            return View();
        }
        [HttpGet("Register")]
        [AllowAnonymous]
        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                return Redirect("/");
            }
            return View();
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _unitOfWork.User.LogoutAsync();
            return Redirect("/");
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModelDto dto, string returnUrl)
        {
            if (await _unitOfWork.User.UserNameOrPasswordInvalid(dto))
            {
                ModelState.AddModelError("LoginError", "Invalid username or password.");
                return View(dto);
            }
            await _unitOfWork.User.LoginAsync(dto);

            TempData["success"] = "Successful login!";
            return LocalRedirect(returnUrl ??= "/");
        }

        [HttpPost("Register")]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModelDto dto, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            if (!(await _unitOfWork.User.GetFirstOrDefaultDtoAsync(x => x.Email == dto.Email) is null))
            {
                ModelState.AddModelError("RegisterError", "That email is taken.");
                return View(dto);
            }

            await _unitOfWork.User.RegisterAsync(dto);
            await _unitOfWork.SaveAsync();

            TempData["success"] = "Successful registration!";
            return LocalRedirect(returnUrl ??= "/");
        }
    }
}
