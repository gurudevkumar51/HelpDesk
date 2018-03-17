using HelpDeskCommon.CommonClasses;
using HelpDeskDAL.DataMapper;
using HelpDeskEntities.Account;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpDeskDAL.DataAccess
{
    public class AccountRepository : BaseRepository
    {
        private List<User> GetUserList(string email)
        {
            var type = string.IsNullOrEmpty(email) ? "D" : "C";
            try
            {
                UserMapper objUsrMapper = new UserMapper();
                SqlParameter[] parameters = {
                 new SqlParameter("@Type", type),
                 new SqlParameter("@Email", email)

            };
                IDataReader reader = base.GetReader("Usp_Manage_User", parameters);
                using (reader)
                {
                    return objUsrMapper.Map(reader);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public User login(Login lgn, out string msg)
        {
            msg = "";
            try
            {
                var v = GetUserList(lgn.Email).FirstOrDefault();
                if (v != null)
                {
                    if (string.Compare(GenericClass.Hash(lgn.Password), v.Password) == 0)
                    {
                        v.Password = null;
                        if (v.Status)
                        {
                            UpdateLastLogin(lgn.Email);
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

        private int UpdateLastLogin(string emailID)
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
    }
}
