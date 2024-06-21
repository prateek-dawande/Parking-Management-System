namespace ParkingManagementSystem.Models.Domain
{
    public class AvailabilityDate
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public bool IsAvailable { get; set; }
        public int ParkingSlotId { get; set; }
        public ParkingSlot ParkingSlot { get; set; }
    }
}
