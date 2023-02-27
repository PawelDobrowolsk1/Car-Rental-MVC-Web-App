using Car_Rental_MVC.Models;

namespace Car_Rental_MVC.Repositories
{
    public interface ICarRepository
    {
        CarModelDto GetCarById(int carId); // ok
        IEnumerable<CarModelDto> GetAll(); // ok
        Task RentCarAsync(string email, int carId); //ok
        IEnumerable<CarModelDto> RentedCarsByUser(string email); //ok
        void GiveBackCar(string email, int carId);
        void AddCar(CarModelDto car); //ok
        void DeleteCar(int carId); //ok
    }
}
