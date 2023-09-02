using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RentCar_AspNetCore7.Models
{
    public class Booking
    {
        [Key]
        public int Id
        {
            get; set;
        }

        [Required]
        [MaxLength(255)]
        public string FirstName
        {
            get; set;
        }

        [Required]
        [MaxLength(255)]
        public string LastName
        {
            get; set;
        }

        [Required]
        [EmailAddress]
        public string Email
        {
            get; set;
        }

        [Required]
        [MaxLength(15)]
        public string Phone
        {
            get; set;
        }

        [ForeignKey("Car")]
        public int RentedCarId
        {
            get; set;
        }
        public Car RentedCar
        {
            get; set;
        }

        [ForeignKey("City")]
        public int? RentalCityId
        {
            get; set;
        }
        public City RentalCity
        {
            get; set;
        }

        [ForeignKey("City")]
        public int? DropoffCityId
        {
            get; set;
        }
        public City? DropoffCity
        {
            get; set;
        }

        [Required]
        public DateTime RentalStartDate
        {
            get; set;
        }

        [Required]
        public DateTime RentalEndDate
        {
            get; set;
        }

        [Required]
        public decimal TotalPrice
        {
            get; set;
        }

        [Required]
        public bool IsApproved
        {
            get; set;
        }
    }
}
