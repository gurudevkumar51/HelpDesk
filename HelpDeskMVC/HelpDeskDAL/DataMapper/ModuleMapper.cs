using HelpDeskEntities.Modules;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpDeskDAL.DataMapper
{
   public class ModuleMapper
    {
        public List<Modules> Map(IDataReader reader)
        {
            List<Modules> mdls = new List<Modules>();
            while (reader.Read())
            {
                Modules mdl = new Modules();
                mdl.ModuleID = Convert.ToInt32(reader["ModuleID"] == DBNull.Value ? 0 : reader["ModuleID"]);
                mdl.Module = reader["ModuleName"] == DBNull.Value ? "" : reader["ModuleName"].ToString();
                mdls.Add(mdl);
            }
            return mdls;
        }
    }
}
