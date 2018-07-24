using HelpDeskEntities.Ticket;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpDeskDAL.DataMapper
{
    class TicketNatureMapper
    {
        public List<TicketNature> Map(IDataReader reader)
        {
            List<TicketNature> tns = new List<TicketNature>();
            while (reader.Read())
            {
                TicketNature tn = new TicketNature();
                tn.NatureID = Convert.ToInt32(reader["ID"] == DBNull.Value ? 0 : reader["ID"]);
                tn.Nature = reader["Nature"] == DBNull.Value ? "" : reader["Nature"].ToString();
                tn.Status = Convert.ToBoolean(reader["IsActive"] == DBNull.Value ? 1 : reader["IsActive"]);
                tns.Add(tn);
            }
            return tns;
        }
    }
}
