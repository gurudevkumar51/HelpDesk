using HelpDeskCommon.CommonClasses;
using HelpDeskEntities.Account;
using System;
using System.Data.SqlClient;

namespace HelpDeskDAL.DataAccess
{
    public class AccountRepository : BaseRepository
    {                      
        public int UpdateLastLogin(string emailID)
        {
            var flag = 0;
            try
            {
                SqlParameter[] parameters = {
                         new SqlParameter("@Type", "E"),
                         new SqlParameter("@Email", emailID)
                        };
                flag = ExecuteNonQuery("SP_Manage_User", parameters);
            }
            catch (Exception ex)
            {
                return 0;
            }
            return flag;
        }

        public int ChangePassword(ChangePassword chpwd, out string msg)
        {
            msg = "";
            var flag = 0;
            try
            {
                SqlParameter[] parameters = {
                         new SqlParameter("@Type", "G"),
                         new SqlParameter("@Email", chpwd.UserEmail),
                         new SqlParameter("@Password", GenericClass.Hash(chpwd.NewPassword))//GenericClass.Hash(lgn.Password)
                        };
                flag = ExecuteNonQuery("SP_Manage_User", parameters);
            }
            catch (Exception ex)
            {
                msg = "unable to change password due to " + ex.Message;
                return 0;
            }
            return flag;
        }
    }
}
