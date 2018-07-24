using HelpDeskDAL.DataAccess;
using HelpDeskEntities.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpDeskBAL.User
{
  public  class UserGroupBusiness
    {
        private UserGroupRepository ugRepo = new UserGroupRepository();
        public List<UserGroup> GetAllUserGroups()
        {
            return ugRepo.GetAllUserGroups();
        }

        public int AddUserGroup(UserGroup ug, out string msg)
        {
            return ugRepo.AddUserGroup(ug, out msg);
        }
    }
}
