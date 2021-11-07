using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;

namespace DataBase_DAL
{
    public class InvoiceDB : IData<Invoice>
    {
        string _default_invoice_path;
        StorageFile file_instance;

        public InvoiceDB()
        {
            _default_invoice_path = "InvoiceDB";
            Check_Create_DatabaseAsync();
        }
        public async Task Add_New_Element_To_DatabaseAsync(Invoice element)
        // => await Windows.Storage.FileIO.AppendTextAsync(file_instance, element.Pack());
        {
            List<Invoice> invoice_List = await Get_All();
            if (invoice_List.Find(invoice => invoice.Equals(element)) != null)
                throw new Invoice_Exception("Invoice Already Exists");   ////////////Console.log()
            invoice_List.Add(element);
            await Update_DatabaseAsync(invoice_List);
        }
        public async Task<List<Invoice>> Get_All()
        {
            Check_Create_DatabaseAsync();
            return (await FileIO.ReadLinesAsync(file_instance)).Select(l => UnPack(l)).ToList();
        }

        public async Task Remove_Element_From_Database(Invoice element)
        {
            List<Invoice> invoice_list = await Get_All();
            if (invoice_list.Find(invoice => invoice.Equals(element)) == null)
                throw new Invoice_Exception("Invoice does'nt Exists");  ////////////Console.log()
            invoice_list.Remove(element);
            await Update_DatabaseAsync(invoice_list);
        }

        public async Task Update_DatabaseAsync(List<Invoice> element_list)
        {
            Check_Create_DatabaseAsync();
            await FileIO.WriteLinesAsync(file_instance, element_list.Select(s => Pack(s)).ToList());
        }
        public async void Check_Create_DatabaseAsync()
        {
            var folder = ApplicationData.Current.LocalFolder;
            if (await folder.TryGetItemAsync(_default_invoice_path + ".txt") == null)
                await folder.CreateFileAsync(_default_invoice_path + ".txt", CreationCollisionOption.FailIfExists);
            file_instance = await folder.GetFileAsync(_default_invoice_path + ".txt");
        }

        public static string Pack(Invoice element)
        {
            if (element.Item_For_Invoice is Book)
            {
                Book b = element.Item_For_Invoice as Book;
                return $"{element.Invoice_ID}~{CustomerDB.Pack(element.Customer)}~{element.Date}~{ItemDB.Pack(b)}~{element.Price}";
            }
            Journal j = element.Item_For_Invoice as Journal;
            return $"{element.Invoice_ID}~{CustomerDB.Pack(element.Customer)}~{element.Date}~{ItemDB.Pack(j)}~{element.Price}";
        }
        public static Invoice UnPack(string invoice_stringified)
        {
                string[] parameters = invoice_stringified.Split('~');
                ItemType item_type = (ItemType)Enum.Parse(typeof(ItemType), parameters[3].Split('!')[0]);
                switch (item_type)
                {
                    case ItemType.Book:
                        Book b = ItemDB.UnPack(parameters[3]) as Book;
                        return new Invoice(int.Parse(parameters[0]), CustomerDB.UnPack(parameters[1]), DateTime.Parse(parameters[2]), b, double.Parse(parameters[4]));
                    case ItemType.Journal:
                        Journal J = ItemDB.UnPack(parameters[3]) as Journal;
                        return new Invoice(int.Parse(parameters[0]), CustomerDB.UnPack(parameters[1]), DateTime.Parse(parameters[2]), J, double.Parse(parameters[4]));
                }
                throw new Exception("Invoice Exception");
        }
    }
}
