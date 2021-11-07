using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;

namespace DataBase_DAL
{
    public class CustomerDB : IData<Customer>
    {
        string _default_customer_path;
        StorageFile file_instance;

        public CustomerDB()
        {
            _default_customer_path = "CustomerDB";
            Check_Create_DatabaseAsync();
        }
        public async Task Add_New_Element_To_DatabaseAsync(Customer element)
        {
            List<Customer> customer_List = await Get_All();
            if (customer_List.Find(customer => customer.Equals(element)) != null)
                throw new Customer_Exception("Customer Already Exists");   ////////////Console.log()
            customer_List.Add(element);
            await Update_DatabaseAsync(customer_List);
        }
        public async void Check_Create_DatabaseAsync()
        {
            var folder = ApplicationData.Current.LocalFolder;
            if (await folder.TryGetItemAsync(_default_customer_path + ".txt") == null)
            {
                await folder.CreateFileAsync(_default_customer_path + ".txt", CreationCollisionOption.FailIfExists);
            }
                file_instance = await folder.GetFileAsync(_default_customer_path + ".txt");
        }

        public async Task<List<Customer>> Get_All()
        {
            Check_Create_DatabaseAsync();
            return (await FileIO.ReadLinesAsync(file_instance)).Select(l => UnPack(l)).ToList();
        }


        public async Task Remove_Element_From_Database(Customer element)
        {
            List<Customer> customer_list = await Get_All();
            if (customer_list.Find(customer => customer.Equals(element)) == null)
                throw new Customer_Exception("Customer does'nt Exists");  ////////////Console.log()
            customer_list.Remove(element); 
            await Update_DatabaseAsync(customer_list);
        }

        public async Task Update_DatabaseAsync(List<Customer> element_list)
        {
            Check_Create_DatabaseAsync();
            await FileIO.WriteLinesAsync(file_instance, element_list.Select(s => Pack(s)).ToList());
        }

        public static string Pack(Customer element)
        => $"{element.First_name}@{element.Last_name}@{element.ID}@{element._birthday}@{element.Discount}";

        public static Customer UnPack(string customer_stringified)
        {
            string[] parameters = customer_stringified.Split('@');
            return new Customer(parameters[0], parameters[1], int.Parse(parameters[2]), DateTime.Parse(parameters[3]), double.Parse(parameters[4]));
        }

    }
}
