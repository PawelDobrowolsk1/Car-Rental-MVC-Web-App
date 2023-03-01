using Car_Rental_MVC.Entities;
using Car_Rental_MVC.Models;

namespace Car_Rental_MVC.Repositories.IRepositories
{
    public interface ICarRepository : IRepository<Car, CarModelDto>
    {
        Task AddCarAsync(CarModelDto carDto);
        Task UpdateAsync(CarModelDto carDto);
        Task DeleteCarAsync(int carId);

        Task RentCarAsync(string email, int carId);
        Task<IEnumerable<CarModelDto>> RentedCarsByUser(string email);
        Task ReturnCar(string email, int carId);
    }
}
