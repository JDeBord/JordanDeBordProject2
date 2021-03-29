using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JordanDeBordProject2.Models.Entities
{
    public class PaidMovie
    {
        public int Id { get; set; }

        
        public double SalePrice { get; set; }

        [DataType(DataType.Date), Required]
        public DateTime SaleDate { get; set; } = DateTime.Now.Date;

        [Required]
        public int TimesWatched { get; set; }

        [Required]
        public int MovieId { get; set; }
        public Movie Movie { get; set; }

        [Required]
        public int ProfileId { get; set; }
        public Profile Profile { get; set; }
    }
}
