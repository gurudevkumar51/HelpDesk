using HelpDeskBAL.Module;
using HelpDeskBAL.User;
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
        private UserGroupBusiness ugBAL = new UserGroupBusiness();
        public AddUserViewModel()
        {
            User = new User();
            UserGroupList = new List<UserGroup>();
            UserGroupList = ugBAL.GetAllUserGroups();
            //foreach (var name in Enum.GetNames(typeof(User_Group)))
            //{
            //    UserGroup ug = new UserGroup();
            //    ug.GroupID = (int)Enum.Parse(typeof(User_Group), name);
            //    ug.UsrGroup = name;
            //    UserGroupList.Add(ug);
            //}
            Modules = new List<Modules>();
            Modules = mdlb.AllModuleList();
        }
        public User User { get; set; }
        public List<UserGroup> UserGroupList { get; set; }
        public List<Modules> Modules { get; set; }
    }
}