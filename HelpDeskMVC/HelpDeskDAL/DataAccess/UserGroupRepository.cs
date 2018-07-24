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
    public class UserGroupRepository : BaseRepository
    {
        public List<UserGroup> GetAllUserGroups()
        {
            var type = "A";
            try
            {
                UserGroupMapper objUsrMapper = new UserGroupMapper();
                SqlParameter[] parameters = {
                 new SqlParameter("@Type", type)
                };
                IDataReader reader = base.GetReader("SP_Manage_UserGroup", parameters);
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

        public int AddUserGroup(UserGroup ug, out string msg)
        {
            var flag = 0; msg = "";
            try
            {
                SqlParameter[] parameters =
                {
                    new SqlParameter("@User_Group",ug.UsrGroup),
                    new SqlParameter("@Type","B")
                };
                flag = ExecuteNonQuery("SP_Manage_UserGroup", parameters);
            }
            catch (Exception ex)
            {
                flag = 0;
                msg = "Unable to add due to " + ex.Message;
            }
            return flag;
        }
    }
}
