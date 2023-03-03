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

namespace Car_Rental_MVC.Areas.Client.Controllers
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
        public IActionResult Index()
        {
            return View();
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


    }
}
