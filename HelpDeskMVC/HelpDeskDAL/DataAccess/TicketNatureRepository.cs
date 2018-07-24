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
  public  class TicketNatureRepository:BaseRepository
    {
        public List<TicketNature> GetAllTicketNature()
        {
            var type = "A";
            try
            {
                TicketNatureMapper objUsrMapper = new TicketNatureMapper();
                SqlParameter[] parameters = {
                 new SqlParameter("@Type", type)
                };
                IDataReader reader = base.GetReader("SP_Manage_Ticket_Nature", parameters);
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

        public int AddTicketNature(TicketNature tn, out string msg)
        {
            var flag = 0; msg = "";
            try
            {
                SqlParameter[] parameters =
                {
                    new SqlParameter("@TicketNature",tn.Nature),
                    new SqlParameter("@Type","B")
                };
                flag = ExecuteNonQuery("SP_Manage_Ticket_Nature", parameters);
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
