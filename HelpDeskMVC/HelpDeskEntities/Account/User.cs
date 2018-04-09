using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HelpDeskEntities.Account
{
    public class User
    {
        public User()
        {
            UserGroup = new UserGroup();
            Modules = new List<HelpDeskEntities.Modules.Modules>();
        }
        public int UID { get; set; }
        [Required(ErrorMessage ="Please Enter Name")]
        public string Name { get; set; }

        [Display(Name = "Email ID")]
        [Required(ErrorMessage = "Enter your Email ID")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Please enter a valid Email ID")]
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
        public string MemberSince { get; set; }
        public UserGroup UserGroup { get; set; }

        public List<HelpDeskEntities.Modules.Modules> Modules { get; set; }
        [Display(Name = "Status")]
        public Boolean Status { get; set; }

        [Display(Name = "Last Login")]
        public DateTime last_Login { get; set; }
    }
}
