using Car_Rental_MVC.Models;
using Car_Rental_MVC.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Car_Rental_MVC.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly IUserRepository _userRepository;

        public AuthenticationController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
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
            await HttpContext.SignOutAsync();
            return Redirect("/");
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModelDto dto, string returnUrl)
        {
            if (_userRepository.UserNameOrPasswordInvalid(dto))
            {
                ModelState.AddModelError("LoginError", "Invalid username or password.");
                return View(dto);
            }

            var claimsPrincipal = _userRepository.GenerateClaimsPrincipal(dto);

            await HttpContext.SignInAsync(claimsPrincipal);
            
            TempData["success"] = "Successful login!";
            return LocalRedirect(returnUrl ??= "/");
        }

        [HttpPost("Register")]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegisterModelDto dto, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (_userRepository.EmailInUse(dto.Email))
                {
                    ModelState.AddModelError("RegisterError", "That email is taken.");
                    return View(dto);
                }

                _userRepository.RegisterUser(dto);

                TempData["success"] = "Successful registration!";
              
                return LocalRedirect(returnUrl ??= "/");
            }

            return View(dto);
        }
    }
}
