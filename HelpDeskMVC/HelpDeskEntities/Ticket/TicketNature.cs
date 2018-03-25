using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpDeskEntities.Ticket
{
    public class TicketNature
    {
        [Required(ErrorMessage = "Select Nature of Ticket")]
        public int NatureID { get; set; }
        
        public string Nature { get; set; }
    }
}
