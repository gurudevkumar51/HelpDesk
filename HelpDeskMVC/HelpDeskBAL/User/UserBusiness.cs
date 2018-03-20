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

        public int AddNewUser(HelpDeskEntities.Account.User usr, List<int> mdls, out string msg)
        {
            usr.Password = GenericClass.Hash(usr.Password);
            var flag = usrRepo.AddNewUser(usr, out msg);
            if (mdls != null)
            {
                if (flag > 0 && mdls.Count > 0)
                {
                    var currentUser = GetUserList(usr.EmailID).FirstOrDefault();
                    foreach (var m in mdls)
                    {
                        usrRepo.UpdateUserModule(m, currentUser.UID, out msg);
                    }
                }
            }
            return flag;
        }

        public int UpdateUserProfile(HelpDeskEntities.Account.User usr, out string msg)
        {
            return usrRepo.UpdateUserProfile(usr, out msg);
        }
    }
}
