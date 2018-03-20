using HelpDeskBAL.Module;
using HelpDeskEntities;
using HelpDeskEntities.Account;
using HelpDeskEntities.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HelpDeskMVC.Models
{
    public class AddUserViewModel
    {
        private ModuleBAL mdlb = new ModuleBAL();
        public AddUserViewModel()
        {
            UserGroupList = new List<UserGroup>();
            foreach (var name in Enum.GetNames(typeof(User_Group)))
            {
                UserGroup ug = new UserGroup();
                ug.GroupID = (int)Enum.Parse(typeof(User_Group), name);
                ug.UsrGroup = name;
                UserGroupList.Add(ug);
            }
            Modules = new List<HelpDeskEntities.Modules.Modules>();
            Modules = mdlb.AllModuleList();
        }
        public User User { get; set; }
        public List<UserGroup> UserGroupList { get; set; }
        public List<Modules> Modules { get; set; }
    }
}