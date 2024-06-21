namespace ParkingManagementSystem.Models
{
    public class ConfirmationViewModel
    {
        public string UserName { get; set; }
        public string CarNumber { get; set; }
        public string SlotNumber { get; set; }
        public DateTime BookingDate { get; set; }
    }
}
