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
    }
}
