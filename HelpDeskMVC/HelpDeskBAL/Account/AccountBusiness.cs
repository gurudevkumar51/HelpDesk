using HelpDeskBAL.User;
using HelpDeskCommon.CommonClasses;
using HelpDeskDAL.DataAccess;
using HelpDeskEntities.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace HelpDeskBAL.Account
{
    public class AccountBusiness
    {
        private AccountRepository accRepo = new AccountRepository();
        private UserBusiness usrRepo = new UserBusiness();
        private string[] CurrentUser = GenericClass.CsvToStringArray(HttpContext.Current.User.Identity.Name);

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
                        msg = "Wrong Password/Email user";
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

        public Boolean ChangePassword(ChangePassword chp, out string msg)
        {
            chp.UserEmail = CurrentUser[0];
            var flag = false; msg = "";
            var v = usrRepo.GetUserList(CurrentUser[0]).FirstOrDefault();

            if (string.Compare(GenericClass.Hash(chp.Password), v.Password) == 0)
            {
                flag = accRepo.ChangePassword(chp, out msg) > 0 ? true : false;
                msg = flag ? "Successfully changed the password" : msg;
            }
            else
            {
                flag = false;
                msg = "Old password is not Correct";
            }
            
            return flag;
        }

        
    }
}
