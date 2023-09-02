using System.ComponentModel.DataAnnotations;
using RentCar_AspNetCore7.Utilities;

namespace RentCar_AspNetCore7.Models
{
    public class City
    {
        [Key]
        public int Id
        {
            get; set;
        }

        [Required]
        [MaxLength(255)]
        public string Name
        {
            get; set;
        }

        [MaxLength(255)]
        public string? Slug
        {
            get; set;
        }
        public bool IsUpdated { get; set; } = false;
    }
    
}
