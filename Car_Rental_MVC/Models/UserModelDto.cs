using Car_Rental_MVC.Entities;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Car_Rental_MVC.Models
{
    public class UserModelDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "The Email field is required")]
        [EmailAddress]
        public string Email { get; set; }

        [DisplayName("New password")]
        [MinLength(6, ErrorMessage = "Password must be a minimum length of '6'")]
        [DataType(DataType.Password)]
        public string? NewPassword { get; set; }

        [DisplayName("Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The password and confirmation password do not match.")]
        [DataType(DataType.Password)]
        public string? ConfirmNewPassword { get; set; }

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

        public List<CarModelDto>? Cars { get; set; }
    }
}
