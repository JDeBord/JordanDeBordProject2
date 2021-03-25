using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JordanDeBordProject2.Models.ViewModels
{
    public class AddGenreVM
    {
        [Display(Name = "Movie Id")]
        public int Id { get; set; }

        [Display(Name = "Movie Title")]
        public string Title { get; set; }

        [Display(Name = "Current Genres")]
        public string Genres{ get; set; }

        [Display(Name = "Genre to Add")]
        public int NewGenreId { get; set; }
    }
}
