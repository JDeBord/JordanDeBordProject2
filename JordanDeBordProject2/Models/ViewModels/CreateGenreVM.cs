using JordanDeBordProject2.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JordanDeBordProject2.Models.ViewModels
{
    public class CreateGenreVM
    {
        [Display(Name = "Genre Name")]
        public string GenreName { get; set; }

        public Genre GetGenreInstance()
        {
            return new Genre
            {
                Id = 0,
                Name = GenreName
            };
        }
    }
}
