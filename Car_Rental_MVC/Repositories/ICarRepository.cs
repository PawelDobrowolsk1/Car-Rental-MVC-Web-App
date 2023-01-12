using Car_Rental_MVC.Models;

namespace Car_Rental_MVC.Repositories
{
    public interface ICarRepository
    {
        CarModelDto GetCarById(int carId);
        IEnumerable<CarModelDto> GetAll();
        Task RentCarAsync(string email, int carId);
        IEnumerable<CarModelDto> RentedCarsByUser(string email);
        void GiveBackCar(string email, int carId);
        void AddCar(CarModelDto car);
        void DeleteCar(int carId);
    }
}
