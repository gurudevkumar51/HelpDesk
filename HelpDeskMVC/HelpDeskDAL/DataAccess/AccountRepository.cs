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
