using Car_Rental_MVC.Entities;

namespace Car_Rental_MVC.Models
{
    public class UserModelDto
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int NumberRentedCars { get; set; }
        public string Role { get; set; }

    }
}
