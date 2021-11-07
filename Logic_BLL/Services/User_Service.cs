using Common;
using DataBase_DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic_BLL
{
    public class User_Service : ILogic<User>
    {
        readonly IData<User> _user_db;
        public User_Service(IData<User> userDb = null) // null for Release and Dummy for testing
        {
            if (userDb == null)
                userDb = new UserDB();
            _user_db = userDb;
        }

        /// <summary>
        /// Create new User for your company
        /// </summary>
        /// <param name="user_name">Uniq user-name</param>
        /// <param name="password">secret password</param>
        /// <param name="premission_level">level of the new user</param>
        /// <returns>an instance of the new user</returns>
        public async Task<User> Generate_UserAsync(string user_name, string password, Level premission_level)
        {
            if (user_name == null || password == null)
            {
                Service_Manager.Instance.Log_Service_Instance.Add_New_Exception_To_Log("Cannot create new user if User name or password is null!");
                throw new ArgumentNullException("Cannot create new user if User name or password is null!");
            }
            User new_user = new User(user_name, password, premission_level);
            await Add_New_Element_To_Database(new_user);
            return new_user;
        }

        /// <summary>
        /// Update User of the company
        /// </summary>
        /// <param name="old">old instance of the user</param>
        /// <param name="password">new Password</param>
        /// <param name="permission_level">new Permission level</param>
        /// <returns>an instance of the new user</returns>
        public async Task<User> Update_UserAsync(User old, string password, Level permission_level)
        {
            if (password == null)
            {
                Service_Manager.Instance.Log_Service_Instance.Add_New_Exception_To_Log("Cannot update user if password is null!");
                throw new ArgumentNullException("Cannot update user if password is null!");
            }
            List<User> user_list = await Get_AllAsync();
            User new_wanted_user = user_list.Find(u => u.User_Name == old.User_Name);
            new_wanted_user.UpdateUser(password, permission_level);
            await Update_Database(user_list);
            return new_wanted_user;
        }

        /// <summary>
        /// Search for user by user-name
        /// </summary>
        /// <param name="partial_username">user partial username</param>
        /// <returns>list of all user matches the patial usernames</returns>
        public async Task<List<User>> User_SearchAsync(string partial_username)
        {
            if (partial_username == null)
            {
                Service_Manager.Instance.Log_Service_Instance.Add_New_Exception_To_Log("searching for null as partial name is'nt possible!");
                throw new ArgumentNullException("searching for null as partial name is'nt possible!");
            }
            return (await Get_AllAsync()).FindAll(user => user.User_Name.ToLower().Contains(partial_username.ToLower()));
        }

        /// <summary>
        /// User Log in Page Logic
        /// </summary>
        /// <param name="user_name">Users user-name</param>
        /// <param name="password">Users Passworrd</param>
        /// <returns>an instance of the user logged in</returns>
        public async Task<User> Log_In(string user_name, string password)
        {
            if (user_name == null || password == null)
            {
                Service_Manager.Instance.Log_Service_Instance.Add_New_Exception_To_Log("Cannot Search user if User name or password is null!");
                throw new ArgumentNullException("Cannot Search user if User name or password is null!");
            }
            User input_user = (await Get_AllAsync()).Find(user => user.User_Name == user_name);
            if (input_user == null)
            {
                Service_Manager.Instance.Log_Service_Instance.Add_New_Exception_To_Log("User Not Found");
                throw new User_Exception("User Not Found");
            }
            if (input_user.Password != password)
            {
                Service_Manager.Instance.Log_Service_Instance.Add_New_Exception_To_Log("Wrong password");
                throw new User_Exception("Wrong password");
            }
            else
                return input_user;
        }

        // ILogic's Interface Functions
        public async Task Add_New_Element_To_Database(User element)
        {
            if (element == null)
            {
                Service_Manager.Instance.Log_Service_Instance.Add_New_Exception_To_Log("User entered can't be null!");
                throw new ArgumentNullException("User entered can't be null!");
            }
            try
            {
                await _user_db.Add_New_Element_To_DatabaseAsync(element);
            }
            catch (User_Exception user_exception)
            {
                Service_Manager.Instance.Log_Service_Instance.Add_New_Exception_To_Log(user_exception.Message);
                throw user_exception;
            }
        }

        public async Task<List<User>> Get_AllAsync()
        => (await _user_db.Get_All()).OrderBy(user => user.User_Name).ToList();

        public async Task Remove_Element_From_Database(User element)
        {
            if (element == null)
            {
                Service_Manager.Instance.Log_Service_Instance.Add_New_Exception_To_Log("User entered can't be null!");
                throw new ArgumentNullException("User entered can't be null!");
            }
            try
            {
                await _user_db.Remove_Element_From_Database(element);
            }
            catch (User_Exception user_exception)
            {
                Service_Manager.Instance.Log_Service_Instance.Add_New_Exception_To_Log(user_exception.Message);
                throw user_exception;
            }
        }

        public async Task Update_Database(List<User> elements_list)
        {
            if (elements_list == null)
            {
                Service_Manager.Instance.Log_Service_Instance.Add_New_Exception_To_Log("User list entered can't be null!");
                throw new ArgumentNullException("User list entered can't be null!");
            }
            await _user_db.Update_DatabaseAsync(elements_list);
        }
    }
}
