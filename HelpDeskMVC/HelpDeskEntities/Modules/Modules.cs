using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpDeskEntities.Modules
{
  public  class Modules
    {
        [Required(ErrorMessage = "Select Module of Ticket")]
        public int ModuleID { get; set; }
        
        public string Module { get; set; }
    }
}
