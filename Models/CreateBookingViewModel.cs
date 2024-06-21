using System.ComponentModel.DataAnnotations;

namespace ParkingManagementSystem.Models
{
    public class CreateBookingViewModel
    {
        public int UserId { get; set; }
        public int SlotId { get; set; }
        public DateTime BookingDate { get; set; }
        [Required]
        public string CarNumber { get; set; }
    }
}
