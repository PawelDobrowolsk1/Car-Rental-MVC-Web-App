using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Car_Rental_MVC.Entities
{
    public class RentCarInfo
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int CarId { get; set; }
        public bool IsGivenBack { get; set; }

        public virtual User User { get; set; }
        public virtual Car Car { get; set; }

    }
}
