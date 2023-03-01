using AngleSharp.Text;
using Car_Rental_MVC.Exceptions;
using Car_Rental_MVC.Models;
using Car_Rental_MVC.Repositories;
using Car_Rental_MVC.Repositories.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Diagnostics.Eventing.Reader;
using System.Security.Claims;

namespace Car_Rental_MVC.Controllers
{

    [Authorize]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult BackToPreviousPage(string returnUrl)
        {
            if (string.IsNullOrEmpty(returnUrl))
            {
                return Redirect("/");
            }
            return LocalRedirect(returnUrl);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Cars()
        {
            return View(await _unitOfWork.Car.GetAllDtoAsync());
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Details(int carId, string returnUrl)
        {
            TempData["ReturnUrl"] = returnUrl;
            var car = await _unitOfWork
                .Car
                .GetFirstOrDefaultDtoAsync(x => x.Id == carId) ?? throw new NotFoundException("Car not found.");

            if (car == null)
                return NotFound();

            return View(car);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> RentCar(int carId)
        {
            if (carId == null)
            {
                throw new NotFoundException("Car not found");
            }
            await _unitOfWork.Car.RentCarAsync(User.Identity.Name, carId);
            await _unitOfWork.SaveAsync();

            TempData["success"] = "Car successfully rented ";
            return Redirect("Cars");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> RentedCars()
        {
            return View(await _unitOfWork.Car.RentedCarsByUser(User.Identity.Name));
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GiveBackCar(int carId)
        {
            await _unitOfWork.Car.ReturnCar(User.Identity.Name, carId);
            await _unitOfWork.SaveAsync();

            TempData["success"] = "Car successfully returned";
            return Redirect("RentedCars");
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Manager")]
        public IActionResult AddCar()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Manager")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCar(CarModelDto carDto)
        {
            if (!ModelState.IsValid)
            {
                return View(carDto);
            }
            await _unitOfWork.Car.AddCarAsync(carDto);
            await _unitOfWork.SaveAsync();

            return Redirect("/Home/Cars");
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> EditCar(int carId)
        {
            return View(
                await _unitOfWork.Car.GetFirstOrDefaultDtoAsync(x => x.Id == carId) ?? throw new NotFoundException("Car not found.")
                );
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Manager")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCar(int carId, CarModelDto carDto)
        {
            carDto.Id = carId;
            if (!ModelState.IsValid)
            {
                return View(carDto);
            }
            await _unitOfWork.Car.UpdateAsync(carDto);
            await _unitOfWork.SaveAsync();

            return Redirect("/Home/Cars");
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCar(int carId)
        {
            await _unitOfWork.Car.DeleteCarAsync(carId);
            await _unitOfWork.SaveAsync();

            TempData["success"] = "Car successfully deleted ";
            return Redirect("/Home/Cars");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UsersInfo()
        {
            return View(await _unitOfWork.User.GetAllUsersDto());
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UserInfoDetails(int userId, string returnUrl)
        {
            TempData["ReturnUrl"] = returnUrl;
            return View(await _unitOfWork.User.GetUserDetails(userId));
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditUserProfileByAdmin(int userId, string returnUrl)
        {
            TempData["ReturnUrl"] = returnUrl;
            return View(await _unitOfWork.User.GetUserDetails(userId));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUserProfileByAdmin(int userId, UserModelDto userDto)
        {
            userDto.Id = userId;
            ModelState.Remove("Cars");
            if (!ModelState.IsValid)
            {
                return View(userDto);
            }
            await _unitOfWork.User.UpdateAsync(userDto);
            await _unitOfWork.SaveAsync();

            return Redirect("/Home/UsersInfo");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> EditUserProfileByUser(string returnUrl)
        {
            TempData["ReturnUrl"] = returnUrl;
            int userId = int.Parse(User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value);

            return View(await _unitOfWork.User.GetUserDetails(userId));
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUserProfileByUser(UserModelDto userDto)
        {
            userDto.Id = int.Parse(User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value);
            userDto.Email = User.Identity.Name;
            ModelState.Remove("Email");
            ModelState.Remove("Role");
            ModelState.Remove("Cars");
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
