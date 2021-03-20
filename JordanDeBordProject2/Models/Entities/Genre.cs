using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JordanDeBordProject2.Models.Entities
{
    public class Genre
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string Name { get; set; }

        public ICollection<MovieGenre> GenreMovies { get; set; }
            = new List<MovieGenre>();
    }
}
