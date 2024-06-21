using System.ComponentModel.DataAnnotations;

namespace ParkingManagementSystem.Models.Domain
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        [Required]
        [StringLength(50)]
        public string UserName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [StringLength(50)]
        public string Password { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; }
    }
}
