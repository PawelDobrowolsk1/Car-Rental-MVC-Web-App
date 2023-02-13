using Car_Rental_MVC.Entities;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Car_Rental_MVC.Models
{
    public class UserModelDto
    {
        [Required(ErrorMessage = "The Email field is required")]
        [EmailAddress]
        public string Email { get; set; }

        [DisplayName("First Name")]
        [Required(ErrorMessage = "The First Name field is required")]
        public string FirstName { get; set; }

        [DisplayName("Last Name")]
        [Required(ErrorMessage = "The Last Name field is required")]
        public string LastName { get; set; }

        [DisplayName("Phone number")]
        [Phone, MaxLength(9)]
        public string? ContactNumber { get; set; }

        [DisplayName("City")]
        public string? City { get; set; }

        [DisplayName("Street")]
        public string? Street { get; set; }

        [DisplayName("Postal code")]
        public string? PostalCode { get; set; }

        public string Role { get; set; }

        [DisplayName("Rented Cars")]
        public int NumberRentedCars { get; set; }
    }
}
