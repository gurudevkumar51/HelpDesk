using HelpDeskDAL.DataMapper;
using HelpDeskEntities.Ticket;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpDeskDAL.DataAccess
{
    public class TicketRepository : BaseRepository
    {
        public int AddNewTicket(Ticket tkt, out string msg)
        {
            var InsertedID = 0; msg = "";
            try
            {
                #region Sql Parameters
                SqlParameter[] parameters = {
                    new SqlParameter("@ModuleID",tkt.TicketModule.ModuleID),
                    new SqlParameter("@NatureID",tkt.Nature.NatureID),
                    new SqlParameter("@TktDescription",tkt.TicketDescription),
                    new SqlParameter("@CreatedBy",tkt.CreatedBy),
                    new SqlParameter("@StatusID",tkt.Status.ID),
                    new SqlParameter("@Type","A"),
                    new SqlParameter("@id",0)
                };
                parameters[6].Direction = ParameterDirection.Output;
                #endregion

                InsertedID = ExecuteNonQueryWithReturnValue("SP_Manage_Ticket", "@id", parameters);
                msg = InsertedID > 0 ? "New Ticket Created" : "Unable to create ticket try again later";
            }
            catch (Exception ex)
            {
                msg = "Unable to create due to: " + ex.Message;
                return 0;
            }
            return InsertedID;
        }

        public List<Ticket> AllTicket(int? UID)
        {
            var Type = UID != null ? "C" : "B";
            try
            {
                TicketMapper objModuleMapper = new TicketMapper();
                SqlParameter[] parameters =
                    {
                    new SqlParameter("@Type",Type),
                    new SqlParameter("@CreatedBy",UID)
                };

                IDataReader reader = base.GetReader("SP_Manage_Ticket", parameters);
                using (reader)
                {
                    return objModuleMapper.Map(reader);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<Ticket> TicketByID(int tktID)
        {
            try
            {
                TicketMapper objModuleMapper = new TicketMapper();
                SqlParameter[] parameters =
                    {
                    new SqlParameter("@Type","F"),
                    new SqlParameter("@TktID",tktID)
                };

                IDataReader reader = base.GetReader("SP_Manage_Ticket", parameters);
                using (reader)
                {
                    return objModuleMapper.Map(reader);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //Updating ticket status like Open, InProgress or Closed
        public int UpdateTicketStatus(int TktID, int StatusID)
        {
            var flag = 0;
            try
            {
                #region Sql Parameters
                SqlParameter[] parameters = {
                    new SqlParameter("@TktID",TktID),
                    new SqlParameter("@StatusID",StatusID),
                    new SqlParameter("@Type","D")
                };
                #endregion

                flag = ExecuteNonQuery("SP_Manage_Ticket", parameters);
            }
            catch (Exception ex)
            {
                return 0;
            }
            return flag;
        }

        //Assigning Ticket to any Super User or HelpDesk User
        public int AssignTicketToUser(int TktID, int AssignedTo, int AssignedBy, string comment)
        {
            var flag = 0;
            try
            {
                #region Sql Parameters
                SqlParameter[] parameters = {
                    new SqlParameter("@TktID",TktID),
                    new SqlParameter("@AssignedTo",AssignedTo),
                    new SqlParameter("@AssignedBy",AssignedBy),
                    new SqlParameter("@EsclationComment",comment),
                    new SqlParameter("@Type","E")
                };
                #endregion

                flag = ExecuteNonQuery("SP_Manage_Ticket", parameters);
            }
            catch (Exception ex)
            {
                return 0;
            }
            return flag;
        }

        public int SetTicketPriority(int tktID, int PriorityID, out string msg)
        {
            msg = "";
            var flag = 0;
            try
            {
                SqlParameter[] parameters = {
                    new SqlParameter("@TktID",tktID),
                    new SqlParameter("@TktPriority",PriorityID),
                    new SqlParameter("@Type","G")
                };
                flag = ExecuteNonQuery("SP_Manage_Ticket", parameters);
            }
            catch (Exception ex)
            {
                msg = "Can't Update Priority Due to: " + ex.Message;
                return flag;
            }
            return flag;
        }
    }
}
