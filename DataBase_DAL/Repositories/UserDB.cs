using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;

namespace DataBase_DAL
{
    public class UserDB : IData<User>
    {
        readonly string _default_user_path;
        StorageFile file_instance;

        public UserDB()
        {
            _default_user_path = "UserDB";
            Check_Create_DatabaseAsync();
            
        }

        public async Task Add_New_Element_To_DatabaseAsync(User element)
        {
            List<User> user_List = await Get_All();
            if (user_List.Find(user => user.Equals(element)) != null)
                throw new User_Exception("User Already Exists");
            user_List.Add(element);
            await Update_DatabaseAsync(user_List);
        }
        public async void Check_Create_DatabaseAsync()
        {
            var folder = ApplicationData.Current.LocalFolder;
            if (await folder.TryGetItemAsync(_default_user_path + ".txt") == null)
                await folder.CreateFileAsync(_default_user_path + ".txt", CreationCollisionOption.FailIfExists);
            file_instance = await folder.GetFileAsync(_default_user_path + ".txt");
        }

        public async Task<List<User>> Get_All()
        {
            Check_Create_DatabaseAsync();
            return (await FileIO.ReadLinesAsync(file_instance)).Select(l => UnPack(l)).ToList();
        }

        public async Task Remove_Element_From_Database(User element)
        {
            List<User> user_list = await Get_All();
            if (user_list.Find(user => user.Equals(element)) == null)
                throw new User_Exception("User does'nt Exists");  ////////////Console.log()
            user_list.Remove(element);
            await Update_DatabaseAsync(user_list);
        }

        public static string Pack(User element)
        => $"{element.User_Name}~{element.Password}~{element.Permission_Level}";

        public static User UnPack(string user_stringified)
        {
            string[] parameters = user_stringified.Split('~');
            return new User(parameters[0], parameters[1], (Level)Enum.Parse(typeof(Level), parameters[2]));
        }

        public async Task Update_DatabaseAsync(List<User> element_list)
        {
            Check_Create_DatabaseAsync();
            await FileIO.WriteLinesAsync(file_instance, element_list.Select(s => Pack(s)).ToList());
        }
    }
}
