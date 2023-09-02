using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using RentCar_AspNetCore7.Utilities;

namespace RentCar_AspNetCore7.Models
{
    public class Car
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

        [NotMapped]
        public IFormFile UploadedPhoto
        {
            get; set;
        }
        public string? Photo
        {
            get; set;
        }

        [Required]
        public int Doors
        {
            get; set;
        }

        [Required]
        public int Seats
        {
            get; set;
        }

        [Required]
        [MaxLength(10)]
        public string Transmission
        {
            get; set;
        }

        [Required]
        public int Year
        {
            get; set;
        }

        [Required]
        public decimal PricePerDay
        {
            get; set;
        }

        [Required]
        public bool IsActive
        {
            get; set;
        }

        [Required]
        public DateTime AvailableDate
        {
            get; set;
        }
        public int CityId
        {
            get; set;
        }
        public City City
        {
            get; set;
        }
    }
}
