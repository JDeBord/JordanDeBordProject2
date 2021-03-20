using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JordanDeBordProject2.Models.Entities
{
    public class MovieGenre
    {
        public int Id { get; set; }

        [Required]
        public int GenreId { get; set; }
        public Genre Genre { get; set; }

        [Required]
        public int MovieId { get; set; }
        public Movie Movie { get; set; }
    }
}
