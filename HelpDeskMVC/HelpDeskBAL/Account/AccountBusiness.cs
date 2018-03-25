using HelpDeskBAL.User;
using HelpDeskCommon.CommonClasses;
using HelpDeskDAL.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpDeskBAL.Account
{
    public class AccountBusiness
    {
        private AccountRepository accRepo = new AccountRepository();
        private UserBusiness usrRepo = new UserBusiness();
        public HelpDeskEntities.Account.User login(HelpDeskEntities.Account.Login lgn, out string msg)
        {
            msg = "";
            try
            {
                var v = usrRepo.GetUserList(lgn.Email).FirstOrDefault();
                if (v != null)
                {
                    if (string.Compare(GenericClass.Hash(lgn.Password), v.Password) == 0)
                    {
                        v.Password = null;
                        if (v.Status)
                        {
                            accRepo.UpdateLastLogin(lgn.Email);
                            return v;
                        }
                        else
                        {
                            msg = "Your Account Is deactivated contact administrator";
                            return null;
                        }
                    }
                    else
                    {
                        msg = "You have entered incorrect password";
                        return null;
                    }
                }
                else
                {
                    msg = "You are not registered with us";
                    return null;
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                return null;
            }
        }
    }
}
