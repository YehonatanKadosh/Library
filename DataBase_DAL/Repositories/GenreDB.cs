using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;

namespace DataBase_DAL
{
    public class GenreDB : IData<Genre>
    {
        string _default_genre_path;
        StorageFile file_instance;

        public GenreDB()
        {
            _default_genre_path = "GenreDB";
            Check_Create_DatabaseAsync();
        }

        public async Task Add_New_Element_To_DatabaseAsync(Genre element)
        // => await Windows.Storage.FileIO.AppendTextAsync(file_instance, element.Pack());
        {
            List<Genre> genre_List = await Get_All();
            if (genre_List.Find(genre => genre.Equals(element)) != null)
                throw new Genre_Exception("Genre Already Exists");
            genre_List.Add(element);
            await Update_DatabaseAsync(genre_List);
        }

        public async void Check_Create_DatabaseAsync()
        {
            var folder = ApplicationData.Current.LocalFolder;
            if (await folder.TryGetItemAsync(_default_genre_path + ".txt") == null)
                await folder.CreateFileAsync(_default_genre_path + ".txt", CreationCollisionOption.FailIfExists);
            file_instance = await folder.GetFileAsync(_default_genre_path + ".txt");
        }

        public async Task<List<Genre>> Get_All()
        {
            Check_Create_DatabaseAsync();
            return (await FileIO.ReadLinesAsync(file_instance)).Select(l => UnPack(l)).ToList();
        }
        public async Task Remove_Element_From_Database(Genre element)
        {
            List<Genre> genre_list = await Get_All();
            if (genre_list.Find(genre => genre.Equals(element)) == null)
                throw new Genre_Exception("Genre does'nt Exists");  ////////////Console.log()
            genre_list.Remove(element);
            await Update_DatabaseAsync(genre_list);
        }

        public async Task Update_DatabaseAsync(List<Genre> element_list)
        {
            Check_Create_DatabaseAsync();
            await FileIO.WriteLinesAsync(file_instance, element_list.Select(s => Pack(s)).ToList());
        }

        public static string Pack(Genre element)
        => $"{element.Name}${element.Type}";

        public static Genre UnPack(string genre_stringified)
        => new Genre(genre_stringified.Split('$')[0], (GenreType)Enum.Parse(typeof(GenreType), genre_stringified.Split('$')[1]));

    }
}
