using Car_Rental_MVC.Exceptions;
using Car_Rental_MVC.Models;
using Car_Rental_MVC.Repositories;
using Car_Rental_MVC.Repositories.IRepositories;
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
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ICarRepository carRepository, IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _carRepository = carRepository;
            _userRepository = userRepository;
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
            //return View(_carRepository.GetAll());
            return View(await _unitOfWork.Car.GetAllDtoAsync());
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Details(int carId, string returnUrl)
        {
            TempData["ReturnUrl"] = returnUrl;
            //var car = _carRepository.GetCarById(carId);
            var car = await _unitOfWork.Car.GetFirstOrDefaultDtoAsync(x => x.Id == carId);

            if(car == null)
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
            //await _carRepository.RentCarAsync(UserId, carId);
            await _unitOfWork.Car.RentCarAsync(User.Identity.Name, carId);
            await _unitOfWork.SaveAsync();

            TempData["success"] = "Car successfully rented ";
            return Redirect("Cars");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> RentedCars()
        {
            //return View(_carRepository.RentedCarsByUser(User.Identity.Name));
            return View(await _unitOfWork.Car.RentedCarsByUser(User.Identity.Name));
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GiveBackCar(int carId)
        {
            //_carRepository.GiveBackCar(User.Identity.Name, carId);
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

            //_carRepository.AddCar(carDto);
            await _unitOfWork.Car.AddCarAsync(carDto);

            return Redirect("/Home/Cars");
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCar(int carId)
        {
            //_carRepository.DeleteCar(carId);
            await _unitOfWork.Car.DeleteCarAsync(carId);
            await _unitOfWork.SaveAsync();

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
            return View(_userRepository.GetUserInfoDetails(email));
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult EditUserProfileByAdmin(string email, string returnUrl)
        {
            TempData["ReturnUrl"] = returnUrl;
            TempData["UserEmail"] = email;
            return View(_userRepository.GetUserInfoDetails(email));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public IActionResult EditUserProfileByAdmin(UserModelDto userDto, string UserEmail)
        {
            userDto.Email = UserEmail;
            ModelState.Remove("Cars");
            ModelState.Remove("Email");
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
            ModelState.Remove("Cars");
            if(!ModelState.IsValid)
            {
                return View(userDto);
            }
            
            _userRepository.SaveEditedUserProfile(userDto);

            return Redirect("/");
        }
    }
}
