using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace DataBase_DAL
{
    public class LogDB
    {
        string _default_Log_path;
        StorageFile file_instance;
        public LogDB()
        {
            _default_Log_path = "Log";
            Check_Create_Database();
        }
        public async void Check_Create_Database()
        {
            var folder = ApplicationData.Current.LocalFolder;
            if (await folder.TryGetItemAsync(_default_Log_path + ".txt") == null)
                await folder.CreateFileAsync(_default_Log_path + ".txt", CreationCollisionOption.FailIfExists);
            file_instance = await folder.GetFileAsync(_default_Log_path + ".txt");
        }

        public async void Add_New_Element_To_DatabaseAsync(string new_element)
        {
            Check_Create_Database();
            await FileIO.AppendTextAsync(file_instance, new_element + "\n");
        }
    }
}
