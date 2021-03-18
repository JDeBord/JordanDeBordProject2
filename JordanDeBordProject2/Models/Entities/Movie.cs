using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace JordanDeBordProject2.Models.Entities
{
    public class Movie
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Title { get; set; }

        [Required]
        [Range(1832, 2031)]
        public int Year { get; set; }

        public int LengthInMinutes { get; set; }

        public double Price { get; set; }

        public string IMDB_URL { get; set; }
    }
}
