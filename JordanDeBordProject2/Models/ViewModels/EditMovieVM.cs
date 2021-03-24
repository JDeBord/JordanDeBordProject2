using JordanDeBordProject2.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JordanDeBordProject2.Models.ViewModels
{
    public class EditMovieVM
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public int Year { get; set; }

        [Display(Name = "Length in Minutes")]
        public int? LengthInMinutes { get; set; }

        public double Price { get; set; }

        [Display(Name = "IMDB URL")]
        public string IMDB_URL { get; set; }

        public Movie GetMovieInstance() 
        {
            return new Movie
            {
                Id = this.Id,
                Title = this.Title,
                Year = this.Year,
                LengthInMinutes = this.LengthInMinutes,
                Price = this.Price,
                IMDB_URL = this.IMDB_URL

            };
        }
    }
}
