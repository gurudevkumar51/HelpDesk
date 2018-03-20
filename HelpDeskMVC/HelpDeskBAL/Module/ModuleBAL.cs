using HelpDeskDAL.DataAccess;
using HelpDeskEntities.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpDeskBAL.Module
{
    public class ModuleBAL
    {
        private ModulesRepository mdlRepo = new ModulesRepository();
        public List<Modules> AllModuleList()
        {
            return mdlRepo.AllModuleList();
        }
        public int AddNewModule(Modules mdl, out string msg)
        {
            return mdlRepo.AddNewModule(mdl, out msg);
        }
    }
}
