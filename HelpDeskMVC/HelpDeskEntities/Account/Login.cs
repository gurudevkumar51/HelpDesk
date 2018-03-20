using System;
using System.ComponentModel.DataAnnotations;

namespace HelpDeskEntities.Account
{
    public class Login
    {
        [Display(Name = "Email ID")]
        [Required(ErrorMessage = "Enter your Email ID")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Please enter a valid Email ID")]
        public string Email { get; set; }

        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Enter the Password")]
        public string Password { get; set; }

        //[Display(Name = "Remember me")]
        //public Boolean Rememberme { get; set; }
    }
}
