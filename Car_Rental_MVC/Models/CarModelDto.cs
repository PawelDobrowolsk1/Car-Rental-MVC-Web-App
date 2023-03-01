using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Car_Rental_MVC.Models
{
    public class CarModelDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Category is required")]
        public string Category { get; set; }
        [Required(ErrorMessage = "Make is required")]
        public string Make { get; set; }
        [Required(ErrorMessage = "Model is required")]
        public string Model { get; set; }
        [Required(ErrorMessage = "Engine capacity is required")]
        public decimal? Engine { get; set; }
        [Required(ErrorMessage = "Horsepower is required")]
        public int? Horsepower { get; set; }
        [Required(ErrorMessage = "The year of the car is required")]
        [Range(1886,2023)]
        public int? Year { get; set; }
        [Required(ErrorMessage = "The number of seats is required")]
        public int? Seats { get; set; }
        [Required(ErrorMessage = "The number of doors is required")]
        public int? Doors { get; set; }
        [Required(ErrorMessage = "Car fuel is required")]
        public string? Fuel { get; set; }
        [Required(ErrorMessage = "The transmission is required")]
        public string? Transmission { get; set; }
        [DisplayName("Description:")]
        public string? Description { get; set; }
        public bool Available { get; set; }
    }
}
