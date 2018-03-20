using HelpDeskDAL.DataMapper;
using HelpDeskEntities.Modules;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpDeskDAL.DataAccess
{
    public class ModulesRepository : BaseRepository
    {
        public List<Modules> AllModuleList()
        {            
            try
            {
                ModuleMapper objModuleMapper = new ModuleMapper();
                SqlParameter[] parameters =
                    {
                    new SqlParameter("@Type","B")
                };

                IDataReader reader = base.GetReader("SP_Manage_Modules", parameters);
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
        public int AddNewModule(Modules mdl,out string msg)
        {
            var flag = 0; msg = "";
            try
            {
                SqlParameter[] parameters =
                {
                    new SqlParameter("",mdl.Module),
                    new SqlParameter("","A")
                };
                flag = ExecuteNonQuery("", parameters);
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
