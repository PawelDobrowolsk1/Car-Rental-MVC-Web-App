using AutoMapper;
using Car_Rental_MVC.Entities;
using Car_Rental_MVC.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Car_Rental_MVC.Repositories
{
    public class CarRepository : ICarRepository
    {
        private readonly CarRentalManagerContext _context;
        private readonly IMapper _mapper;

        public CarRepository(CarRentalManagerContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public CarModelDto GetCarById(int carId)
        {
            var car = _context
                .Cars
                .Find(carId);

            var carDto = _mapper.Map<CarModelDto>(car);

            return carDto;
        }

        public IEnumerable<CarModelDto> GetAll()
        {
            var cars = _context.Cars.AsEnumerable();

            var carsDtos = _mapper.Map<IEnumerable<CarModelDto>>(cars);

            return carsDtos;
        }
        public async Task RentCarAsync(string email, int carId)
        {
            var user = _context.Users.SingleOrDefault(u => u.Email == email);
            var car = _context.Cars.SingleOrDefault(c => c.Id == carId);
            if (car != null)
            {
                car.Available = false;
            }

            var rentInfo = new RentCarInfo()
            {
                UserId = user.Id,
                CarId = car.Id,
                IsGivenBack = false
            };

            _context.RentInfo.Add(rentInfo);
            _context.SaveChanges();
        }

        public IEnumerable<CarModelDto> RentedCarsByUser(string email)
        {
            var user = _context
                .Users
                .FirstOrDefault(u => u.Email == email);

            var rentedCarInfo = _context
                .RentInfo
                .Include(c => c.Car)
                .Where(u => u.UserId == user.Id && u.IsGivenBack == false)
                .ToList();

            if (rentedCarInfo.Any())
            {
                var carsDtosList = new List<CarModelDto>();

                for (int i = 0; i < rentedCarInfo.Count(); i++)
                {
                    var carDto = _mapper.Map<CarModelDto>(rentedCarInfo[i].Car);
                    carsDtosList.Add(carDto);
                }

                return carsDtosList;
            }
            return null;
        }

        public void GiveBackCar(string email, int carId)
        {
            var user = _context
                .Users
                .SingleOrDefault(u => u.Email == email);

            var car = _context
                .Cars
                .SingleOrDefault(c => c.Id == carId);

            if (car != null)
            {
                car.Available = true;
            }

            var rentedInfo = _context
                .RentInfo
                .SingleOrDefault(x => x.UserId == user.Id && x.CarId == car.Id && x.IsGivenBack == false);
            if (rentedInfo != null)
            {
                rentedInfo.IsGivenBack = true;
            }

            _context.SaveChanges();
        }

        public void AddCar(CarModelDto carDto)
        {
            var car = _mapper.Map<Car>(carDto);
            _context.Cars.Add(car);
            _context.SaveChanges();
        }

        public void DeleteCar(int carId)
        {
            var car = _context.Cars.SingleOrDefault(c => c.Id == carId);

            _context.Cars.Remove(car);
            _context.SaveChanges();
        }
    }
}
