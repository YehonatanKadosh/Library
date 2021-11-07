using Common;
using DataBase_DAL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic_BLL
{
    public class Item_Service : ILogic<Item>
    {
        readonly IData<Item> _item_db;
        public Item_Service(IData<Item> itemDb = null) // null for Release and Dummy for testing
        {
            if (itemDb == null)
                itemDb = new ItemDB();
            _item_db = itemDb;
        }

        public bool Available(Item element)
        {
            if (element == null)
            {
                Service_Manager.Instance.Log_Service_Instance.Add_New_Exception_To_Log("Element is null!");
                throw new Item_Exception("Element is null!");
            }
            return element.Amount_In_Stock > 0;
        }

        public async Task BuyAsync(Item element)
        {
            if (!Available(element))
            {
                Service_Manager.Instance.Log_Service_Instance.Add_New_Exception_To_Log("There are no copies left!");
                throw new Item_Exception("There are no copies left!");
            }
            else
            {
                List<Item> itemlist = await Get_AllAsync();
                itemlist.Find(iteM => iteM.ISBN == element.ISBN).Restock(-1);
                await Update_Database(itemlist);
            }

        }

        // Generate New Book and insert it to the memory
        public async Task<Book> Generate_New_BookAsync(string name, List<Genre> genre, DateTime date_printed, int amount_in_stock, double price, double discount, string author, string summary)
        {
            if (name == null || genre == null || author == null || summary == null)
            {
                Service_Manager.Instance.Log_Service_Instance.Add_New_Exception_To_Log("Creating an instance of an Book with null as name/ Genre list/ author or summary is'nt possible!");
                throw new ArgumentNullException("Creating an instance of an Book with null as name/ Genre list/ author or summary is'nt possible!");
            }
            int iSBN = 0;
            if ((await Get_AllAsync()).Count != 0)
            {
                List<Item> items = await Get_AllAsync();
                foreach (Item item in items)
                    if (item.ISBN >= iSBN)
                        iSBN = item.ISBN + 1;
            }
            Book new_book = new Book(iSBN, name, ItemType.Book, genre, date_printed, amount_in_stock, price, discount, author, summary);

            await Add_New_Element_To_Database(new_book);
            return new_book;
        }

        // Generate New Journal and insert it to the memory
        public async Task<Journal> Generate_New_JournalAsync(string name, List<Genre> genre, DateTime date_printed, int amount_in_stock, double price, double discount, string author, int issue)
        {
            if (name == null || genre == null || author == null)
                if (name == null || genre == null || author == null)
                {
                    Service_Manager.Instance.Log_Service_Instance.Add_New_Exception_To_Log("Creating an instance of an Journal with null as name/ Genre list or author is'nt possible!");
                    throw new ArgumentNullException("Creating an instance of an Journal with null as name/ Genre list or author is'nt possible!");
                }
            int iSBN = 0;
            if ((await Get_AllAsync()).Count != 0)
            { 
                List<Item> items = await Get_AllAsync();
                foreach (Item item in items)
                    if (item.ISBN >= iSBN)
                        iSBN = item.ISBN + 1;
            }
            Journal new_journal = new Journal(iSBN, name, ItemType.Journal, genre, date_printed, amount_in_stock, price, discount, author, issue);
            await Add_New_Element_To_Database(new_journal);
            return new_journal;
        }

        /// <summary>
        /// Search for Items by partial name
        /// </summary>
        /// <param name="partial_name">the partial items name</param>
        /// <returns>list of items matched to the partial name</returns>
        public async Task<List<Item>> Get_Items_By_Partial_Name(string partial_name)
        {
            if (partial_name == null)
            {
                Service_Manager.Instance.Log_Service_Instance.Add_New_Exception_To_Log("searching for null as partial name is'nt possible!");
                throw new ArgumentNullException("searching for null as partial name is'nt possible!");
            }
            return (await Get_AllAsync()).FindAll(item => item.Name.ToLower().Contains(partial_name.ToLower()));
        }
        /// Same as the last one but searches from the item_list and not threw the database
        public List<Item> Get_Items_By_Partial_Name(string partial_name, List<Item> item_list)
        {
            if (partial_name == null || item_list == null)
            {
                Service_Manager.Instance.Log_Service_Instance.Add_New_Exception_To_Log("searching for null as partial name or item list is'nt possible!");
                throw new ArgumentNullException("searching for null as partial name or item list is'nt possible!");
            }
            return item_list.FindAll(item => item.Name.ToLower().Contains(partial_name.ToLower()));
        }

        /// <summary>
        /// Search for Items by Date
        /// </summary>
        /// <param name="start">Starting Date range</param>
        /// <param name="end">Ending Date range</param>
        /// <returns>list of all Items matching to the Dates searches</returns>
        public async Task<List<Item>> Get_Items_By_Date(DateTime start, DateTime end)
        => (await Get_AllAsync()).FindAll(item => (start <= item.Date_Printed && item.Date_Printed <= end));
        /// Same as the last one but searches from the item_list and not threw the database
        public List<Item> Get_Items_By_Date(DateTime start, DateTime end, List<Item> item_list)
        {
            if (item_list == null)
            {
                Service_Manager.Instance.Log_Service_Instance.Add_New_Exception_To_Log("searching for null as item list is'nt possible!");
                throw new ArgumentNullException("searching for null as item list is'nt possible!");
            }
            return item_list.FindAll(item => (start <= item.Date_Printed && item.Date_Printed <= end));
        }

        /// <summary>
        /// Search Items by Genre
        /// </summary>
        /// <param name="genre_name">genre's name</param>
        /// <returns>list of items mached by genre</returns>
        public async Task<List<Item>> Get_Items_By_Genre(Genre genre_name)
        {
            if (genre_name == null)
            {
                Service_Manager.Instance.Log_Service_Instance.Add_New_Exception_To_Log("searching for null as genre name is'nt possible!");
                throw new ArgumentNullException("searching for null as genre name is'nt possible!");
            }
            return (await Get_AllAsync()).FindAll(item => item.Genre_List.Contains(genre_name));
        }
        /// Same as the last one but searches from the item_list and not threw the database
        public List<Item> Get_Items_By_Genre(Genre genre_name, List<Item> item_list)
        {
            if (genre_name == null || item_list == null)
            {
                Service_Manager.Instance.Log_Service_Instance.Add_New_Exception_To_Log("searching for null as genre name or item list is'nt possible!");
                throw new ArgumentNullException("searching for null as genre name or item list is'nt possible!");
            }
            return item_list.FindAll(item => item.Genre_List.Contains(genre_name));
        }

        /// <summary>
        /// Search Items by isbn
        /// </summary>
        /// <param name="isbn">identify the item</param>
        /// <returns>the matching item or null if it doesnt exsit</returns>
        public async Task<Item> Get_Item_By_ISBN(int isbn)
        => (await Get_AllAsync()).Find(item => item.ISBN.ToString().Contains(isbn.ToString()));
        
        /// <summary>
        /// Update an Exsisting item
        /// </summary>
        /// <param name="old">old item</param>
        /// <param name="amount_added">new amount</param>
        /// <param name="price">new price</param>
        /// <param name="discount">new item discount</param>
        public async Task Update_Item(Item old, int amount_added, double price, double discount)
        {
            List<Item> items = await Get_AllAsync();
            Item selected = items.Find(iteM => iteM.ISBN == old.ISBN);
            selected.Restock(amount_added - old.Amount_In_Stock);
            selected.Price = price;
            selected.Discount = discount;
            await Update_Database(items);
        }

        // ILogic's Interface Functions
        public async Task Add_New_Element_To_Database(Item element)
        {
            if (element == null)
            {
                Service_Manager.Instance.Log_Service_Instance.Add_New_Exception_To_Log("Item entered can't be null!");
                throw new ArgumentNullException("Item entered can't be null!");
            }
            try
            {
                await _item_db.Add_New_Element_To_DatabaseAsync(element);
            }
            catch (Item_Exception item_exception)
            {
                Service_Manager.Instance.Log_Service_Instance.Add_New_Exception_To_Log(item_exception.Message);
                throw item_exception;
            }
        }

        public async Task<List<Item>> Get_AllAsync()
        => (await _item_db.Get_All()).OrderBy(item => item.Name).ToList();

        public async Task Remove_Element_From_Database(Item element)
        {
            if (element == null)
            {
                Service_Manager.Instance.Log_Service_Instance.Add_New_Exception_To_Log("Item entered can't be null!");
                throw new ArgumentNullException("Item entered can't be null!");
            }
            try
            {
                await _item_db.Remove_Element_From_Database(element);
            }
            catch (Item_Exception item_exception)
            {
                Service_Manager.Instance.Log_Service_Instance.Add_New_Exception_To_Log(item_exception.Message);
                throw item_exception;
            }
        }

        public async Task Update_Database(List<Item> elements_list)
        {
            if (elements_list == null)
            {
                Service_Manager.Instance.Log_Service_Instance.Add_New_Exception_To_Log("Item lisr entered can't be null!");
                throw new ArgumentNullException("Item lisr entered can't be null!");
            }
            await _item_db.Update_DatabaseAsync(elements_list);
        }
    }
}
