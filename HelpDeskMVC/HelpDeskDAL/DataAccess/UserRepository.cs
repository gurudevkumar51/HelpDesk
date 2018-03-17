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
   public class UserRepository : BaseRepository
    {
        public List<User> GetUserList(string email)
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

        public int AddNewUser(User usr, out string msg)
        {
            var flag = 0; msg = "";
            try
            {
                SqlParameter[] parameters = {
                         new SqlParameter("@Type", "A"),
                         new SqlParameter("@Email", usr.EmailID),
                         new SqlParameter("@PhoneNumber", usr.ContactNo),
                         new SqlParameter("@Name", usr.Name),
                         new SqlParameter("@Password", usr.Password),
                         new SqlParameter("@UserGroupID", usr.UserGroup.GroupID)
                        };
                flag = ExecuteNonQuery("SP_Manage_User", parameters);
            }
            catch (Exception ex)
            {
                msg = "Unable to add " + ex.Message;
                return flag;
            }
            return flag;
        }

        public int UpdateUserProfile(User usr, out string msg)
        {
            var flag = 0; msg = "";
            try
            {
                SqlParameter[] parameters = {
                         new SqlParameter("@Type", "B"),
                         new SqlParameter("@Email", usr.EmailID),
                         new SqlParameter("@PhoneNumber", usr.ContactNo),
                         new SqlParameter("@Name", usr.Name),
                         new SqlParameter("@UserGroupID", usr.UserGroup.GroupID)
                        };
                flag = ExecuteNonQuery("SP_Manage_User", parameters);
            }
            catch (Exception ex)
            {
                msg = "Unable to update due to " + ex.Message;
                return flag;
            }
            return flag;
        }
    }
}
