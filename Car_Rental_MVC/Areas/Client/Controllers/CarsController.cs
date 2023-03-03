using Car_Rental_MVC.Exceptions;
using Car_Rental_MVC.Models;
using Car_Rental_MVC.Repositories.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Car_Rental_MVC.Areas.Client.Controllers
{
    public class CarsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CarsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            return View(await _unitOfWork.Car.GetAllDtoAsync());
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Details(int id, string returnUrl)
        {
            TempData["ReturnUrl"] = returnUrl;
            var car = await _unitOfWork
                .Car
                .GetFirstOrDefaultDtoAsync(x => x.Id == id) ?? throw new NotFoundException("Car not found.");

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
            return RedirectToAction("Index");
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

            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> EditCar(int id)
        {
            return View(
                await _unitOfWork.Car.GetFirstOrDefaultDtoAsync(x => x.Id == id) ?? throw new NotFoundException("Car not found.")
                );
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Manager")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCar(int id, CarModelDto carDto)
        {
            carDto.Id = id;
            if (!ModelState.IsValid)
            {
                return View(carDto);
            }
            await _unitOfWork.Car.UpdateAsync(carDto);
            await _unitOfWork.SaveAsync();

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCar(int id)
        {
            await _unitOfWork.Car.DeleteCarAsync(id);
            await _unitOfWork.SaveAsync();

            TempData["success"] = "Car successfully deleted ";
            return RedirectToAction("Index");
        }
    }
}
