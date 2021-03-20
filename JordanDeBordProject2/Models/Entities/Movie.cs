﻿using System;
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
        public int Year { get; set; }

        public int LengthInMinutes { get; set; }

        [Required]
        public double Price { get; set; }

        [Required]
        public string IMDB_URL { get; set; }

        public ICollection<MovieGenre> MovieGenres { get; set; }
            = new List<MovieGenre>();

        public ICollection<PaidMovie> PaidMovies { get; set; }
            = new List<PaidMovie>();
    }
}
