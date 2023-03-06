using Car_Rental_MVC.Models;
using Car_Rental_MVC.Repositories;
using Car_Rental_MVC.Repositories.IRepositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Car_Rental_MVC.Areas.Identity.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public AccountController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return Redirect("/");
            }
            return View();
        }
        [HttpGet]
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

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModelDto dto, string? returnUrl)
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

        [HttpPost]
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

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UsersInfo()
        {
            return View(await _unitOfWork.User.GetAllUsersDto());
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UserInfoDetails(int id, string returnUrl)
        {
            TempData["ReturnUrl"] = returnUrl;
            return View(await _unitOfWork.User.GetUserDetails(id));
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditUser(int id, string returnUrl)
        {
            TempData["ReturnUrl"] = returnUrl;
            return View(await _unitOfWork.User.GetUserDetails(id));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(int id, UserModelDto userDto)
        {
            userDto.Id = id;
            if (!ModelState.IsValid)
            {
                return View(userDto);
            }
            await _unitOfWork.User.UpdateAsync(userDto);
            await _unitOfWork.SaveAsync();

            return Redirect($"/Identity/Account/UserInfoDetails/{id}");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Manage(string returnUrl)
        {
            TempData["ReturnUrl"] = returnUrl;
            int userId = int.Parse(User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value);

            return View(await _unitOfWork.User.GetFirstOrDefaultDtoAsync(x => x.Id == userId));
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Manage(UserModelDto userDto)
        {
            userDto.Id = int.Parse(User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value);
            userDto.Email = User.Identity.Name;
            ModelState.Remove("Email");
            ModelState.Remove("Role");
            if (!ModelState.IsValid)
            {
                return View(userDto);
            }
            await _unitOfWork.User.UpdateAsync(userDto);
            await _unitOfWork.SaveAsync();

            return Redirect("/");
        }
    }
}
