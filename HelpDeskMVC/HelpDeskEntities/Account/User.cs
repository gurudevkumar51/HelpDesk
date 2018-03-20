using System;
using System.ComponentModel.DataAnnotations;

namespace HelpDeskEntities.Account
{
    public class User
    {
        public User()
        {
            UserGroup = new UserGroup();
        }
        public int UID { get; set; }
        [Required(ErrorMessage ="Please Enter Name")]
        public string Name { get; set; }

        [Display(Name = "Email ID")]
        [Required(ErrorMessage = "Enter your Email ID")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Please enter a valid Email ID")]
        //[System.Web.Mvc.Remote("CheckExistingEmail", "Miscellaneous", ErrorMessage = "Email already exists!")]
        public string EmailID { get; set; }

        public string ContactNo { get; set; }

        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Enter the Password")]
        [MinLength(8, ErrorMessage = "Minimum 8 characters required")]
        public string Password { get; set; }

        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password and Confirmation Password must match.")]
        [MinLength(8, ErrorMessage = "Minimum 8 characters required")]
        public string ConfirmPassword { get; set; }
        public UserGroup UserGroup { get; set; }

        [Display(Name = "Status")]
        public Boolean Status { get; set; }

        public int ModuleID { get; set; }
        public string Module { get; set; }

        [Display(Name = "Last Login")]
        public DateTime last_Login { get; set; }
    }
}
