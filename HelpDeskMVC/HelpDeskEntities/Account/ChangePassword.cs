using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpDeskEntities.Account
{
    public class ChangePassword
    {

        [Display(Name = "Old Password")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage ="Please Enter Your Old Password")]
        [MinLength(8, ErrorMessage = "Minimum 8 characters required")]
        public string Password { get; set; }

        [Display(Name = "New Password")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Please Enter Your New Password")]
        [MinLength(8, ErrorMessage = "Minimum 8 characters required")]
        public string NewPassword { get; set; }

        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Please Confirm Your New Password")]
        [Compare("NewPassword", ErrorMessage = "Password and Confirmation Password must match.")]
        [MinLength(8, ErrorMessage = "Minimum 8 characters required")]
        public string ConfirmNewPassword { get; set; }
    }
}
