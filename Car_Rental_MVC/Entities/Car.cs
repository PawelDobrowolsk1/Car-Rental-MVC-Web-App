namespace Car_Rental_MVC.Entities
{
    public class Car
    {
        public int Id { get; set; }
        public string Category { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public decimal Engine { get; set; }
        public int Horsepower { get; set; }
        public int Year { get; set; }
        public int Seats { get; set; }
        public int Doors { get; set; }
        public string? Fuel { get; set; }
        public string Transmission { get; set; }
        public string? Description { get; set; }
        public bool Available { get; set; } = true;

        public virtual RentCarInfo RentedCarInfo { get; set; }
    }
}
