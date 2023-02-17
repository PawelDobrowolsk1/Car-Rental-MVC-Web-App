using Car_Rental_MVC.Models;
using Car_Rental_MVC.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Diagnostics.Eventing.Reader;

namespace Car_Rental_MVC.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ICarRepository _carRepository;
        private readonly IUserRepository _userRepository;

        public HomeController(ICarRepository carRepository, IUserRepository userRepository)
        {
            _carRepository = carRepository;
            _userRepository = userRepository;
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
        public IActionResult Cars()
        {
            return View(_carRepository.GetAll());
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Details(int carId, string returnUrl)
        {
            TempData["ReturnUrl"] = returnUrl;
            return View(_carRepository.GetCarById(carId));
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> RentCar(string email, int carId)
        {
            if (email != null && carId != null)
            {
                await _carRepository.RentCarAsync(email, carId);

                TempData["success"] = "Car successfully rented ";
                return Redirect("Cars");
            }

            return Redirect("/");
        }

        [HttpGet]
        [Authorize]
        public IActionResult RentedCars()
        {
            return View(_carRepository.RentedCarsByUser(User.Identity.Name));
        }

        [HttpGet]
        [Authorize]
        public IActionResult GiveBackCar(string email, int carId)
        {
            _carRepository.GiveBackCar(email, carId);

            TempData["success"] = "Car successfully returned";
            return Redirect("RentedCars");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult AddCar()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public IActionResult AddCar(CarModelDto carDto)
        {
            if (!ModelState.IsValid)
            {
                return View(carDto);
            }

            _carRepository.AddCar(carDto);

            return Redirect("/Home/Cars");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult DeleteCar(int carId)
        {
            _carRepository.DeleteCar(carId);

            TempData["success"] = "Car successfully deleted ";
            return Redirect("/Home/Cars");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult UsersInfo()
        {
            return View(_userRepository.GetAllUsers());
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult UserInfoDetails(string email, string returnUrl)
        {
            TempData["ReturnUrl"] = returnUrl;
            var cars = _carRepository.RentedCarsByUser(email);
            return View(_userRepository.GetUserInfoDetails(email));
        }

        [HttpGet]
        [Authorize(Roles ="Admin")]
        public IActionResult EditUserProfileByAdmin(string email, string returnUrl)
        {
            TempData["ReturnUrl"] = returnUrl;
            return View(_userRepository.GetUserInfoDetails(email));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public IActionResult EditUserProfileByAdmin(UserModelDto userDto)
        {
            if (_userRepository.EmailInUse(userDto.Email))
            {
                ModelState.AddModelError("EmailIsTaken", "That email is taken.");
            }
            if (!ModelState.IsValid)
            {
                return View(userDto);
            }

            _userRepository.SaveEditedUserProfile(userDto);

            return Redirect("/Home/UsersInfo");
        }

        [HttpGet]
        [Authorize]
        public IActionResult EditUserProfileByUser(string email, string returnUrl)
        {
            TempData["ReturnUrl"] = returnUrl;
            return View(_userRepository.GetUserInfoDetails(email));
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult EditUserProfileByUser(UserModelDto userDto)
        {
            userDto.Email = User.Identity.Name;
            ModelState.Remove("Email");
            ModelState.Remove("Role");
            if(!ModelState.IsValid)
            {
                return View(userDto);
            }
            
            _userRepository.SaveEditedUserProfile(userDto);

            return Redirect("/");
        }
    }
}
