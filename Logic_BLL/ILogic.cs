using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic_BLL
{
    interface ILogic<T>
    {
        public Task<List<T>> Get_AllAsync();
        public Task Add_New_Element_To_Database(T element);
        public Task Remove_Element_From_Database(T element);
        public Task Update_Database(List<T> elements_list);
    }
}
