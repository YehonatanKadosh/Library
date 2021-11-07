using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataBase_DAL;

namespace Logic_BLL
{
    public class Log_Service
    {
        readonly LogDB _log_db;
        public Log_Service() 
        {
            _log_db = new LogDB();
        }
        public void Add_New_Exception_To_Log(string new_element)
        {
            _log_db.Add_New_Element_To_DatabaseAsync(new_element);
        }
    }
}
