using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Car_Rental_MVC.Models
{
    public class CarModelDto
    {
        public int Id { get; set; }
        [Required]
        public string Category { get; set; }
        [Required]
        public string Make { get; set; }
        [Required]
        public string Model { get; set; }
        [Required(ErrorMessage = "Engine capacity is required")]
        public decimal Engine { get; set; }
        [Required(ErrorMessage = "Horsepower is required")]
        public int Horsepower { get; set; }
        [Required(ErrorMessage = "The year of the car is required")]
        public int Year { get; set; }
        [Required(ErrorMessage = "The number of seats is required")]
        public int Seats { get; set; }
        [Required(ErrorMessage = "The number of doors is required")]
        public int Doors { get; set; }
        [Required(ErrorMessage = "Car fuel is required")]
        public string? Fuel { get; set; }
        [Required(ErrorMessage = "The transmisson is required")]
        public string Transmisson { get; set; }
        [DisplayName("Description:")]
        public string? Description { get; set; }
        public bool Available { get; set; }
    }
}
