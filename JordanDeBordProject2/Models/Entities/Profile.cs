using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace JordanDeBordProject2.Models.Entities
{
    public class Profile
    {
        public int Id { get; set; }

        [MaxLength(12), MinLength(12), Required]
        public string CCNum { get; set; }

        [DataType(DataType.Date), Required]
        public DateTime CCExp { get; set; }
        
        [MaxLength(100), Required]
        public string AddLine1 { get; set; }
        
        [MaxLength(30)]
        public string AddLine2 { get; set; }

        [MaxLength(50), Required]
        public string City { get; set; }

        [MaxLength(2), MinLength(2), Required]
        public string State { get; set; }
        
        [MaxLength(5), MinLength(5), Required]
        public string ZIPCode { get; set; }

        [Required]
        public string ApplicationUserId { get; set; }

        
        public ApplicationUser ApplicationUser { get; set; }

        public ICollection<PaidMovie> PaidMovies { get; set; }
            = new List<PaidMovie>();

        [NotMapped]
        public double TotalAmountSpent 
        {
            get 
            {
                var sum = 0.0;
                foreach (var movie in PaidMovies)
                {
                    sum += movie.Movie.Price;
                }
                return sum;
            } 
        }
    }
}
