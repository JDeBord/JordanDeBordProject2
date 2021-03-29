using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JordanDeBordProject2.Models.ViewModels
{
    public class DetailsMovieVM
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public int? Year { get; set; }

        [Display(Name = "Length In Minutes")]
        public int? LengthInMin { get; set; }

        public double Price { get; set; }
    }
}
