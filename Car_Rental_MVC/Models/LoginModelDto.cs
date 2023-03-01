using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Car_Rental_MVC.Models
{
    public class LoginModelDto
    {
        [Required(ErrorMessage = "The Email field is required")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "The Password field is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DisplayName("Remember me?")]
        public bool IsPersistent { get; set; } = false;
    }
}
