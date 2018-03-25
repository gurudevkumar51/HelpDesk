using HelpDeskEntities.Account;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HelpDeskMVC.Models
{
    public class TicketAssign
    {
        public TicketAssign()
        {
            UserList = new List<User>();
        }

        public int TktID { get; set; }
        public int AssignBy { get; set; }

        [Required(ErrorMessage ="Select User to Assign")]
        public int AssignTo { get; set; }
        [Required(ErrorMessage ="Put your Comment")]
        [DataType(DataType.MultilineText)]
        public string Comment { get; set; }
        public List<User> UserList { get; set; }
    }
}