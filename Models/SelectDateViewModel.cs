using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ParkingManagementSystem.Models
{
    public class SelectDateViewModel
    {
        [Required]
        [DisplayName("Enter Booking Date")]
        public DateTime BookingDate { get; set; }
    }
}
