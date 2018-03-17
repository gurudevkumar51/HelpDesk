using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpDeskEntities.Account
{
    public class UserGroup
    {
        public int GroupID { get; set; }

        [Required(ErrorMessage ="User Group is required")]
        [Display(Name ="User Group")]
        public string UsrGroup { get; set; }
    }
}
