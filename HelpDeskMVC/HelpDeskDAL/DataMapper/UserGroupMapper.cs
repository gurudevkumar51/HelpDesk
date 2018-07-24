using HelpDeskEntities.Account;
using System;
using System.Collections.Generic;
using System.Data;

namespace HelpDeskDAL.DataMapper
{
    class UserGroupMapper
    {
        public List<UserGroup> Map(IDataReader reader)
        {
            List<UserGroup> ugs = new List<UserGroup>();
            while (reader.Read())
            {
                UserGroup ug = new UserGroup();
                ug.GroupID = Convert.ToInt32(reader["ID"] == DBNull.Value ? 0 : reader["ID"]);
                ug.UsrGroup = reader["User_Group"] == DBNull.Value ? "" : reader["User_Group"].ToString();
                ug.DeActivatedDate = reader["DeActivationDate"] == DBNull.Value ? "" : reader["DeActivationDate"].ToString();
                ug.Status = Convert.ToBoolean(reader["IsActive"] == DBNull.Value ? 1 : reader["IsActive"]);
                ugs.Add(ug);
            }
            return ugs;
        }
    }
}
