using AngleSharp.Text;
using Car_Rental_MVC.Exceptions;
using Car_Rental_MVC.Models;
using Car_Rental_MVC.Repositories;
using Car_Rental_MVC.Repositories.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Security.Claims;

namespace Car_Rental_MVC.Areas.Home.Controllers
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
        public async Task<IActionResult> Index()
        {
            return View(await _unitOfWork.Car.GetAllDtoAsync());
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> RentedCars()
        {
            var userId = int.Parse(User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value);
            return View(await _unitOfWork.Car.RentedCarsByUser(userId));
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GiveBackCar(int carId)
        {
            var userId = int.Parse(User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier).Value);

            await _unitOfWork.Car.ReturnCar(userId, carId);
            await _unitOfWork.SaveAsync();

            TempData["success"] = "Car successfully returned";
            return Redirect("RentedCars");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [AllowAnonymous]
        public IActionResult Error(int statusCode)
        {
            if (statusCode == 404)
            {
                return View(new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    ErrorCode = statusCode,
                    ErrorMessage = "The site you looking for is not found. Why don't you go to the "
                });
            }
            else if (statusCode == 500)
            {
                return View(new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    ErrorCode = statusCode,
                    ErrorMessage = "Oopss. We had an issue on our website. Why don't you go to the "
                });
            }
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                ErrorCode = statusCode
            });
        }
    }
}
