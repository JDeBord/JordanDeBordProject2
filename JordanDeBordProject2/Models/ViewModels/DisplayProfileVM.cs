using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JordanDeBordProject2.Models.ViewModels
{
    public class DisplayProfileVM
    {
        [Display(Name="Full Name")]
        public string Name { get; set; }

        [Display(Name = "Full Address")]
        public string Address { get; set; }

        [Display(Name="Credit Card Number")]
        public string CCNum { get; set; }

        [Display(Name = "Credit Card Expiration")]
        [DataType(DataType.Date)]
        public DateTime CCExp { get; set; }

        [Display(Name = "Total Money Spent Watching Movies")]
        public string AmountSpent { get; set; }
    }
}
