using Common;
using DataBase_DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Logic_BLL
{
    public class Genre_Service : ILogic<Genre>
    {
        readonly IData<Genre> _genre_db;

        public Genre_Service(IData<Genre> genreDb = null) // null for Release and Dummy for testing
        {
            if (genreDb == null)
                genreDb = new GenreDB();
            _genre_db = genreDb;
        }

        /// <summary>
        /// Create new genre
        /// </summary>
        /// <param name="name">Genre's Name</param>
        /// <param name="genreType">Genre's type</param>
        /// <returns>instance of the new genre</returns>
        public async Task<Genre> Create_New_GenreAsync(string name, GenreType genreType)
        {
            if (name == null)
            {
                Service_Manager.Instance.Log_Service_Instance.Add_New_Exception_To_Log("Creating an instance of Genre with null as name is'nt possible!");
                throw new ArgumentNullException("Creating an instance of Genre with null as name is'nt possible!");

            }
            Genre new_genre = new Genre(name, genreType);
            await Add_New_Element_To_Database(new_genre);
            return new_genre;
        }


        //Seperation for version 2.0
        /// <summary>
        /// Search genre by type and partial name
        /// </summary>
        /// <param name="partial_name">genre's partial name</param>
        /// <returns>list of the matching genres by partial name</returns>
        public async Task<List<Genre>> Genre_Search_NonfictionAsync(string partial_name)
        {
            if (partial_name == null)
            {
                Service_Manager.Instance.Log_Service_Instance.Add_New_Exception_To_Log("searching for null as partial name is'nt possible!");
                throw new ArgumentNullException("searching for null as partial name is'nt possible!");
            }
            return (await Get_AllAsync()).FindAll(genre => (genre.Name.ToLower().Contains(partial_name.ToLower()) && genre.Type == GenreType.Nonfiction));
        }

        /// <summary>
        /// Search genre by type and partial name
        /// </summary>
        /// <param name="partial_name">genre's partial name</param>
        /// <returns>list of the matching genres by partial name</returns>
        public async Task<List<Genre>> Genre_Search_FictionAsync(string partial_name)
        {
            if (partial_name == null)
            {
                Service_Manager.Instance.Log_Service_Instance.Add_New_Exception_To_Log("searching for null as partial name is'nt possible!");
                throw new ArgumentNullException("searching for null as partial name is'nt possible!");
            }
            return (await Get_AllAsync()).FindAll(genre => (genre.Name.ToLower().Contains(partial_name.ToLower()) && genre.Type == GenreType.Fiction));
        }

        // ILogic's Interface Functions
        public async Task Add_New_Element_To_Database(Genre element)
        {
            if (element == null)
            {
                Service_Manager.Instance.Log_Service_Instance.Add_New_Exception_To_Log("Genre entered can't be null!");
                throw new ArgumentNullException("Genre entered can't be null!");
            }
            try
            {
                await _genre_db.Add_New_Element_To_DatabaseAsync(element);
            }
            catch (Genre_Exception genre_exception)
            {
                Service_Manager.Instance.Log_Service_Instance.Add_New_Exception_To_Log(genre_exception.Message);
                throw genre_exception;
            }
        }

        public async Task<List<Genre>> Get_AllAsync()
        => (await _genre_db.Get_All()).OrderBy(genre => genre.Name).ToList();

        public async Task Remove_Element_From_Database(Genre element)
        {
            if (element == null)
            {
                Service_Manager.Instance.Log_Service_Instance.Add_New_Exception_To_Log("Genre entered can't be null!");
                throw new ArgumentNullException("Genre entered can't be null!");
            }
            try
            {
                await _genre_db.Remove_Element_From_Database(element);
            }
            catch (Genre_Exception genre_exception)
            {
                Service_Manager.Instance.Log_Service_Instance.Add_New_Exception_To_Log(genre_exception.Message);
                throw genre_exception;
            }
        }

        public async Task Update_Database(List<Genre> elements_list)
        {
            if (elements_list == null)
            {
                Service_Manager.Instance.Log_Service_Instance.Add_New_Exception_To_Log("Genre list entered can't be null!");
                throw new ArgumentNullException("Genre list entered can't be null!");
            }
            await _genre_db.Update_DatabaseAsync(elements_list);
        }
    }
}
