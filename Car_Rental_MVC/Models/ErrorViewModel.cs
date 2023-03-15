namespace Car_Rental_MVC.Models
{
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        public int? ErrorCode { get; set; }
        public string? ErrorMessage { get; set; } = string.Empty;

    }
}