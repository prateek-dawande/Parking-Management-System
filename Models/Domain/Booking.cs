namespace ParkingManagementSystem.Models.Domain
{
    public class Booking
    {
        public int BookingId { get; set; }
        public int UserId { get; set; }
        public int SlotId { get; set; }
        public DateTime BookingDate { get; set; }
        public string CarNumber { get; set; }
        public virtual User User { get; set; }
        public virtual ParkingSlot ParkingSlot { get; set; }
    }
}
