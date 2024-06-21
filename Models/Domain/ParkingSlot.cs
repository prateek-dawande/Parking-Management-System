using System.ComponentModel.DataAnnotations;

namespace ParkingManagementSystem.Models.Domain
{
    public class ParkingSlot
    {
        [Key]
        public int SlotId { get; set; }

        public string SlotNumber { get; set; }
        public bool IsAvaiable { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; }

        public ICollection<AvailabilityDate> AvailabilityDates { get; set; } = new List<AvailabilityDate>();
    }
}
