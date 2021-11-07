using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace DataBase_DAL
{
    public class ItemDB : IData<Item>
    {
        string _default_items_path;
        StorageFile file_instance;

        public ItemDB()
        {
            _default_items_path = "ItemDB";
            Check_Create_DatabaseAsync();
        }

        public async Task Add_New_Element_To_DatabaseAsync(Item element)
        {
            List<Item> item_List = await Get_All();
            if (item_List.Find(item => item.Equals(element)) != null)
                throw new Item_Exception("Item Already Exists");   ////////////Console.log()
            item_List.Add(element);
            await Update_DatabaseAsync(item_List);
        }

        public async Task<List<Item>> Get_All()
        {
            Check_Create_DatabaseAsync();
            return (await FileIO.ReadLinesAsync(file_instance)).Select(l => UnPack(l)).ToList();
        }


        public async Task Update_DatabaseAsync(List<Item> element_list)
        {
            Check_Create_DatabaseAsync();
            await FileIO.WriteLinesAsync(file_instance, element_list.Select(s => Pack(s)).ToList());
        }


        public async void Check_Create_DatabaseAsync()
        {
            var folder = ApplicationData.Current.LocalFolder;
            if (await folder.TryGetItemAsync(_default_items_path + ".txt") == null)
                await folder.CreateFileAsync(_default_items_path + ".txt", CreationCollisionOption.FailIfExists);
            file_instance = await folder.GetFileAsync(_default_items_path + ".txt");
        }

        public async Task Remove_Element_From_Database(Item element)
        {
            List<Item> items_list = await Get_All();
            if (items_list.Find(item => item.Equals(element)) == null)
                throw new Item_Exception("Item does'nt Exists");  ////////////Console.log()
            items_list.Remove(element); //Exception if doesnt exist
            await Update_DatabaseAsync(items_list);
        }

        public static string Pack(Item element)
        {
            StringBuilder genre_list = new StringBuilder();
            foreach (Genre genre in element.Genre_List)
                genre_list.Append(GenreDB.Pack(genre) + '^');
            if (element is Book)
            {
                Book book = element as Book;
                return $"{element.Item_Type}!{element.Name}!{element.Author}!{element.Date_Printed}!{element.ISBN}!{genre_list}!{book._summary}!{element.Price}!{element.Discount}!{element.Amount_In_Stock}";
            }
            else
            {
                Journal journal = element as Journal;
                return $"{element.Item_Type}!{element.Name}!{element.Author}!{element.Date_Printed}!{element.ISBN}!{genre_list}!{journal._issue}!{element.Price}!{element.Discount}!{element.Amount_In_Stock}";
            }
        }

        public static Item UnPack(string item_stringified)
        {
            string[] parameters = item_stringified.Split('!');
            List<Genre> genre_list = new List<Genre>();
            if(parameters[5] != "")
            foreach (string genre in parameters[5].Split('^'))
                {
                    if(genre != "")
                        genre_list.Add(GenreDB.UnPack(genre));
                }
            if ((ItemType)Enum.Parse(typeof(ItemType), item_stringified.Split('!')[0]) == ItemType.Journal)
                return new Journal(int.Parse(parameters[4]), parameters[1], (ItemType)Enum.Parse(typeof(ItemType), parameters[0]), genre_list, DateTime.Parse(parameters[3]), int.Parse(parameters[9]), double.Parse(parameters[7]), double.Parse(parameters[8]), parameters[2], int.Parse(parameters[6]));
            else
                return new Book(int.Parse(parameters[4]), parameters[1], (ItemType)Enum.Parse(typeof(ItemType), parameters[0]), genre_list, DateTime.Parse(parameters[3]), int.Parse(parameters[9]), double.Parse(parameters[7]), double.Parse(parameters[8]), parameters[2], parameters[6]);
        }
    }
}
