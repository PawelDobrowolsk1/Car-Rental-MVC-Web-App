using System.Data;

namespace Car_Rental_MVC.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PasswordHash { get; set; }
        public string? ContactNumber { get; set; }
        public string? City { get; set; }
        public string? Street { get; set; }
        public string? PostalCode { get; set; }


        public int RoleId { get; set; }
        public virtual Role Role { get; set; }
        public virtual RentCarInfo RentedCarInfo { get; set; }
    }
}
