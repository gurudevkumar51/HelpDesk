using HelpDeskEntities;
using HelpDeskEntities.Account;
using System;
using System.Collections.Generic;
using System.Data;

namespace HelpDeskDAL.DataMapper
{
    public class UserMapper
    {
        public List<User> Map(IDataReader reader)
        {
            List<User> accs = new List<User>();
            while (reader.Read())
            {
                User acc = new User();

                acc.UID = Convert.ToInt32(reader["UserID"] == DBNull.Value ? 0 : reader["UserID"]);
                acc.Name = reader["Name"] == DBNull.Value ? "" : reader["Name"].ToString();
                acc.EmailID = reader["Email"] == DBNull.Value ? "" : reader["Email"].ToString();
                acc.ContactNo= reader["PhoneNumber"] == DBNull.Value ? "" : reader["PhoneNumber"].ToString();
                acc.Password = reader["UserPassword"] == DBNull.Value ? "" : reader["UserPassword"].ToString();
                acc.last_Login = reader["LastLogin"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(reader["LastLogin"]);                
                acc.UserGroup.GroupID= Convert.ToInt32(reader["UserGroupID"] == DBNull.Value ? 0 : reader["UserGroupID"]);
                acc.UserGroup.UsrGroup = Enum.GetName(typeof(User_Group), acc.UserGroup.GroupID);
                acc.Status = Convert.ToBoolean(reader["IsActive"] == DBNull.Value ? 0 : reader["IsActive"]);
                accs.Add(acc);
            }
            return accs;
        }

        //public List<User> UserNameEmailIDMap(IDataReader reader)
        //{
        //    List<User> accs = new List<User>();
        //    while (reader.Read())
        //    {
        //        User acc = new User();

        //        acc.UID = Convert.ToInt32(reader["UserID"] == DBNull.Value ? 0 : reader["UserID"]);
        //        acc.Name = reader["Name"] == DBNull.Value ? "" : reader["Name"].ToString();
        //        acc.EmailID = reader["Email"] == DBNull.Value ? "" : reader["Email"].ToString();
        //        //acc.ContactNo = reader["PhoneNumber"] == DBNull.Value ? "" : reader["PhoneNumber"].ToString();
        //        //acc.Password = reader["UserPassword"] == DBNull.Value ? "" : reader["UserPassword"].ToString();
        //        //acc.last_Login = reader["LastLogin"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(reader["LastLogin"]);
        //        //acc.UserGroup.GroupID = Convert.ToInt32(reader["UserGroupID"] == DBNull.Value ? 0 : reader["UserGroupID"]);
        //        //acc.UserGroup.UsrGroup = Enum.GetName(typeof(User_Group), acc.UserGroup.GroupID);
        //        acc.Status = Convert.ToBoolean(reader["IsActive"] == DBNull.Value ? 0 : reader["IsActive"]);
        //        accs.Add(acc);
        //    }
        //    return accs;
        //}
    }
}
