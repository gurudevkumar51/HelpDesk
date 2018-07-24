using HelpDeskEntities;
using HelpDeskEntities.Account;
using System;
using System.Collections.Generic;
using System.Data;

namespace HelpDeskDAL.DataMapper
{
    public class UserMapper
    {
        private DataAccess.ModulesRepository mrepo = new DataAccess.ModulesRepository();
        public List<User> Map(IDataReader reader)
        {
            List<User> accs = new List<User>();
            while (reader.Read())
            {
                User acc = new User();

                acc.UID = Convert.ToInt32(reader["UserID"] == DBNull.Value ? 0 : reader["UserID"]);
                acc.Name = reader["Name"] == DBNull.Value ? "" : reader["Name"].ToString();
                acc.EmailID = reader["Email"] == DBNull.Value ? "" : reader["Email"].ToString();
                acc.MemberSince= reader["CreatedDate"] == DBNull.Value ? "" : Convert.ToDateTime(reader["CreatedDate"]).ToString("dd MMM, yyyy");
                acc.ContactNo = reader["PhoneNumber"] == DBNull.Value ? "" : reader["PhoneNumber"].ToString();
                acc.Password = reader["UserPassword"] == DBNull.Value ? "" : reader["UserPassword"].ToString();
                acc.last_Login = reader["LastLogin"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(reader["LastLogin"]);
                acc.UserGroup.GroupID = Convert.ToInt32(reader["UserGroupID"] == DBNull.Value ? 0 : reader["UserGroupID"]);
                //acc.UserGroup.UsrGroup = Enum.GetName(typeof(User_Group), acc.UserGroup.GroupID);
                acc.UserGroup.UsrGroup = reader["User_Group"] == DBNull.Value ? "" : reader["User_Group"].ToString();
                acc.Modules = mrepo.ModuleListForUser(acc.UID);
                acc.Status = Convert.ToBoolean(reader["IsActive"] == DBNull.Value ? 0 : reader["IsActive"]);
                accs.Add(acc);
            }
            return accs;
        }

        public List<UserProfile> ProfileMap(IDataReader reader)
        {
            List<UserProfile> accs = new List<UserProfile>();
            while (reader.Read())
            {
                UserProfile acc = new UserProfile();

                acc.UID = Convert.ToInt32(reader["UserID"] == DBNull.Value ? 0 : reader["UserID"]);
                acc.Name = reader["Name"] == DBNull.Value ? "" : reader["Name"].ToString();
                acc.EmailID = reader["Email"] == DBNull.Value ? "" : reader["Email"].ToString();
                acc.MemberSince = reader["CreatedDate"] == DBNull.Value ? "" : Convert.ToDateTime(reader["CreatedDate"]).ToString("dd MMM, yyyy");
                acc.ContactNo = reader["PhoneNumber"] == DBNull.Value ? "" : reader["PhoneNumber"].ToString();
                acc.last_Login = reader["LastLogin"] == DBNull.Value ? "" : Convert.ToDateTime(reader["LastLogin"]).ToString("dd MMM, yyyy");
                acc.UserGroup.GroupID = Convert.ToInt32(reader["UserGroupID"] == DBNull.Value ? 0 : reader["UserGroupID"]);
                acc.TicketAssigned = Convert.ToInt32(reader["TicketAssigned"] == DBNull.Value ? 0 : reader["TicketAssigned"]);
                //acc.UserGroup.UsrGroup = Enum.GetName(typeof(User_Group), acc.UserGroup.GroupID);
                acc.UserGroup.UsrGroup = reader["User_Group"] == DBNull.Value ? "" : reader["User_Group"].ToString();
                acc.Modules = mrepo.ModuleListForUser(acc.UID);
                acc.Status = Convert.ToBoolean(reader["IsActive"] == DBNull.Value ? 0 : reader["IsActive"]);
                accs.Add(acc);
            }
            return accs;
        }
    }
}
