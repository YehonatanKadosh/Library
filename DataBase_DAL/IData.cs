using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataBase_DAL
{
    public interface IData<T> //Because its a TxT file and not SQL - Base Commands
    {
        /// <summary>
        /// Checks weather the file exists, creates it if not and saving an instance afterward
        /// </summary>
        public void Check_Create_DatabaseAsync();

        /// <summary>
        /// this method asks the repository to get all strings from the file instance
        /// unpack them all to T objects in a list
        /// </summary>
        /// <returns>List of T by unpacking strings</returns>
        public Task<List<T>> Get_All();

        /// <summary>
        /// Adds element T to the Database
        /// </summary>
        /// <param name="element">an object of the repository generic class</param>
        /// <returns>void</returns>
        public Task Add_New_Element_To_DatabaseAsync(T element);

        /// <summary>
        /// get rid of element T within the Database
        /// </summary>
        /// <param name="element">an object of the repository generic class</param>
        /// <returns>void</returns>
        public Task Remove_Element_From_Database(T element);

        /// <summary>
        /// this method asks the repository to set list of strings to the file instance
        /// </summary>
        /// <param name="element_list"> list of objects before packing them </param>
        /// <returns>void</returns>
        public Task Update_DatabaseAsync(List<T> element_list);
    }
}
