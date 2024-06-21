using System.ComponentModel.DataAnnotations;

namespace ParkingManagementSystem.Models
{
    public class UserLoginMV
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
