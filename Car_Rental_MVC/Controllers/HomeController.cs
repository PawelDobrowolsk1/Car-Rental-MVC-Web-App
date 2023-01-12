using Car_Rental_MVC.Models;
using Car_Rental_MVC.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Eventing.Reader;

namespace Car_Rental_MVC.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ICarRepository _carRepository;

        public HomeController(ICarRepository carRepository)
        {
            _carRepository = carRepository;
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
        public IActionResult AddCar(CarModelDto carDto)
        {
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

    }
}
