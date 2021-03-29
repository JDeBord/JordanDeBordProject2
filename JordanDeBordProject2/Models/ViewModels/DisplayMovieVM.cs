using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JordanDeBordProject2.Models.ViewModels
{
    public class DisplayMovieVM
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public int? Year { get; set; }

        [Display(Name = "Length in Minutes")]
        public int? LengthInMinutes { get; set; }

        public string Price { get; set; }

        [Display(Name = "IMDB URL")]
        public string IMDB_URL { get; set; }

        [Display(Name = "Genres")]
        public string Genres { get; set; }
    }
}
