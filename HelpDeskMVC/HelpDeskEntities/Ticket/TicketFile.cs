using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpDeskEntities.Ticket
{
  public  class TicketFile
    {
        public int FileID { get; set; }
        public int TicketLogID { get; set; }
        public int TicketId { get; set; }
        public string FileDisplayName { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public string UploadDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}
