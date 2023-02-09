using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Car_Rental_MVC.Models
{
    public class RegisterModelDto 
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

        [Required(ErrorMessage = "The Password field is required")]
        [MinLength(6, ErrorMessage = "Password must be a minimum length of '6'")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DisplayName("Confirm Password")]
        [Required(ErrorMessage = "The Confirm Password field is required")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public int RoleId { get; set; } = 1;
    }
}
