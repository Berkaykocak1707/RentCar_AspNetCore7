using System.ComponentModel.DataAnnotations;

namespace RentCar_AspNetCore7.Models
{
    public class Admin
    {
        [Key]
        public int Id
        {
            get; set;
        }

        [Required]
        [MaxLength(50)]
        public string Username
        {
            get; set;
        }
        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email
        {
            get; set;
        }
        // Hashed password
        [Required]
        [MaxLength(255)]
        public string Password
        {
            get; set;
        }
    }
}
