using ParkingManagementSystem.Models.Domain;

namespace ParkingManagementSystem.Models
{
    public class SelectSlotViewModel
    {
        public DateTime BookingDate { get; set; }
        public int SelectedSlotId { get; set; }
        public List<ParkingSlot> AvailableSlots { get; set; } = new List<ParkingSlot>();

        //new property
        public List<ParkingSlot> Slots { get; set; } = new List<ParkingSlot>();
    }
}
