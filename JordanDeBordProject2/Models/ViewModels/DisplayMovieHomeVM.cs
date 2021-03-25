using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JordanDeBordProject2.Models.ViewModels
{
    public class DisplayMovieHomeVM
    {
        public int Id { get; set; }

        public string Title { get; set; }

        [Display(Name = "Number of Times Watched")]
        public int NumTimesWatched { get; set; }

    }
}
