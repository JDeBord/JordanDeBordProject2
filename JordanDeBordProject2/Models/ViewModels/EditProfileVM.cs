using JordanDeBordProject2.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JordanDeBordProject2.Models.ViewModels
{
    public class EditProfileVM
    {

        public int ProfileId { get; set; }

        public string UserId { get; set; }

        public string UserName { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Credit Card Number")]
        public string CCNum { get; set; }

        [Display(Name = "Credit Card Expiration")]
        [DataType(DataType.Date)]
        public  DateTime CCExp { get; set; }

        [Display(Name = "Address Line 1")]
        public string AddLine1 { get; set; }

        [Display(Name = "Address Line 2")]
        public string AddLine2 { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        [Display(Name = "ZIP Code")]
        public string ZIPCode { get; set; }

        public Profile GetProfileInstance() 
        {
            return new Profile
            {
                Id = this.ProfileId,
                CCNum = this.CCNum,
                CCExp = this.CCExp,
                AddLine1 = this.AddLine1,
                AddLine2 = this.AddLine2,
                City = this.City,
                State = this.State.ToUpper(),
                ZIPCode = this.ZIPCode
            };
        }

        public ApplicationUser GetUserInstance() 
        {
            return new ApplicationUser
            {
                Id = this.UserId,
                UserName = this.UserName,
                FirstName = this.FirstName,
                LastName = this.LastName
            };
        }


    }
}
