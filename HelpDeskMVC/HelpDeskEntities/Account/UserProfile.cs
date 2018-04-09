using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpDeskEntities.Account
{
  public  class UserProfile
    {
        public UserProfile()
        {
            Modules = new List<HelpDeskEntities.Modules.Modules>();
            UserGroup = new UserGroup();
        }
        public int UID { get; set; }
        public string Name { get; set; }
        public int TicketAssigned { get; set; }
        public string EmailID { get; set; }
        public string ContactNo { get; set; }
        public string MemberSince { get; set; }
        public UserGroup UserGroup { get; set; }
        public List<HelpDeskEntities.Modules.Modules> Modules { get; set; }
        public Boolean Status { get; set; }
        public string last_Login { get; set; }
    }
}
