using HelpDeskCommon.CommonClasses;
using HelpDeskDAL.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpDeskBAL.User
{
   public class UserBusiness
    {
        private UserRepository usrRepo = new UserRepository();

        public List<HelpDeskEntities.Account.User> GetUserList(string email)
        {
            return usrRepo.GetUserList(email);
        }

        public HelpDeskEntities.Account.User GetUserByUID(int UId)
        {
            return usrRepo.GetUserList(null).Where(u => u.UID == UId).FirstOrDefault();//.ToList();
        }

        public List<HelpDeskEntities.Account.User> GetAllSupportUsers()
        {
            return usrRepo.GetUserList(null).Where(u => u.UserGroup.GroupID == 5).ToList();
        }

        public List<HelpDeskEntities.Account.User> AllSupportUsersForModule(int moduleID)
        {
            var uList = usrRepo.GetUserList(null);
            return uList.Where(u => u.Modules.Any(m => m.ModuleID == moduleID) && u.UserGroup.GroupID == 5).ToList();
        }

        // returns All HelpDesk User & Super User on the basis of moduleID
        public List<HelpDeskEntities.Account.User> HelpDeskUserModuleWise(int mdl)
        {
            return usrRepo.HelpDeskUserModuleWise(mdl);
        }

        public int AddNewUser(HelpDeskEntities.Account.User usr, List<int> mdls, out string msg)
        {
            usr.Password = GenericClass.Hash(usr.Password);
            var InsertedId = usrRepo.AddNewUser(usr, out msg);
            if (mdls != null)
            {
                if (InsertedId > 0 && mdls.Count > 0)
                {
                    var currentUser = GetUserList(usr.EmailID).FirstOrDefault();
                    foreach (var m in mdls)
                    {
                        usrRepo.UpdateUserModule(m, currentUser.UID, out msg);
                    }
                }
            }
            return InsertedId;
        }

        public int UpdateUserProfile(HelpDeskEntities.Account.User usr, out string msg)
        {
            return usrRepo.UpdateUserProfile(usr, out msg);
        }
    }
}
