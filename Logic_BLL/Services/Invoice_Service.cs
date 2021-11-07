using Common;
using DataBase_DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic_BLL
{
    public class Invoice_Service : ILogic<Invoice>
    {
        readonly IData<Invoice> _invoice_DB;
        public static int invoice_ID_Counter;
        public Invoice_Service(IData<Invoice> invoiceDb = null) // null for Release and Dummy for testing
        {
            if (invoiceDb == null)
                invoiceDb = new InvoiceDB();
            _invoice_DB = invoiceDb;
        }

        /// <summary>
        /// Create a new invoice
        /// </summary>
        /// <param name="customer">the customer who purchase the item</param>
        /// <param name="item">item purchased</param>
        /// <param name="manager_discount">if there are any special manager discount - default = 0</param>
        /// <returns></returns>
        public async Task<Invoice> Generate_InvoiceAsync(Customer customer, Item item, double manager_discount = 0)
        {
            if (customer == null || item == null)
            {
                Service_Manager.Instance.Log_Service_Instance.Add_New_Exception_To_Log("Creating an instance of an Invoice with null as customer or item is'nt possible!");
                throw new ArgumentNullException("Creating an instance of an Invoice with null as customer or item is'nt possible!");
            }
            try
            {
                await Service_Manager.Instance.Item_Service_Instance.BuyAsync(item);
            }
            catch(Item_Exception item_exception) 
            {
                Service_Manager.Instance.Log_Service_Instance.Add_New_Exception_To_Log(item_exception.Message);
                throw item_exception;
            }

            double correct_disccount = Math.Max(Math.Max(customer.Discount, item.Discount), manager_discount);
            invoice_ID_Counter = (await Service_Manager.Instance.Invoice_Service_Instance.Get_AllAsync()).Count + 1;
            Invoice new_invoice = new Invoice(invoice_ID_Counter, customer, DateTime.Now, item, (item.Price * (1-correct_disccount)));
            await Add_New_Element_To_Database(new_invoice);
            return new_invoice;
        }

        /// <summary>
        /// Search invoice by customer's partial name
        /// </summary>
        /// <param name="partial_name">customer's partial name</param>
        /// <returns>list of all matching invoices by customers partial name</returns>
        public async Task<List<Invoice>> Invoice_Search_By_Customer_Partial_Name(string partial_name)
        {
            if (partial_name == null)
            {
                Service_Manager.Instance.Log_Service_Instance.Add_New_Exception_To_Log("searching for null as partial name is'nt possible!");
                throw new ArgumentNullException("searching for null as partial name is'nt possible!");
            }
            return (await Get_AllAsync()).FindAll(invoice => invoice.Customer.First_name.ToLower().Contains(partial_name.ToLower()));
        }
        /// Same as the last one but searches from the item_list and not threw the database
        public List<Invoice> Invoice_Search_By_Customer_Partial_Name(string partial_name, List<Invoice> invoice_list)
        { 
            if (partial_name == null || invoice_list == null)
            {
                Service_Manager.Instance.Log_Service_Instance.Add_New_Exception_To_Log("searching for null as partial name or invoice list is'nt possible!");
                throw new ArgumentNullException("searching for null as partial name or invoice list is'nt possible!");
            }
            return invoice_list.FindAll(invoice => invoice.Customer.First_name.ToLower().Contains(partial_name.ToLower()));
        }

        /// <summary>
        /// Search invoices by customer's partial id
        /// </summary>
        /// <param name="partial_ID">customer's partial id</param>
        /// <returns>list of all matching invoices by customer's partial id</returns>
        public async Task<List<Invoice>> Invoice_Search_By_Customer_Partial_ID(int partial_ID) 
        => (await Get_AllAsync()).FindAll(invoice => invoice.Customer.ID.ToString().Contains(partial_ID.ToString()));
        /// Same as the last one but searches from the item_list and not threw the database
        public List<Invoice> Invoice_Search_By_Customer_Partial_ID(int partial_ID, List<Invoice> invoice_list)
        => invoice_list.FindAll(invoice => invoice.Customer.ID.ToString().Contains(partial_ID.ToString()));

        /// <summary>
        /// Search for invoice by invoice's partial id
        /// </summary>
        /// <param name="partial_invoice_ID">invoice's partial id</param>
        /// <returns>list of all invoice's matching to the invoice partial id</returns>
        public async Task<List<Invoice>> Invoice_Search_By_Partial_Invoice_ID(int partial_invoice_ID)
        => (await Get_AllAsync()).FindAll(invoice => invoice.Invoice_ID.ToString().Contains(partial_invoice_ID.ToString()));
        /// Same as the last one but searches from the item_list and not threw the database
        public List<Invoice> Invoice_Search_By_Partial_Invoice_ID(int invoice_ID, List<Invoice> invoice_list)
        => invoice_list.FindAll(invoice => invoice.Invoice_ID.ToString().Contains(invoice_ID.ToString()));

        /// <summary>
        /// Search for invoice by date
        /// </summary>
        /// <param name="start">Start date range</param>
        /// <param name="end">End date range</param>
        /// <returns>list of invoices between start and end dates</returns>
        public async Task<List<Invoice>> Invoice_Search_By_Date(DateTime start, DateTime end)
        {
            List<Invoice> invoices = await Get_AllAsync();
            return (await Get_AllAsync()).FindAll(invoice => (start <= invoice.Date && invoice.Date <= end));
        }
        /// Same as the last one but searches from the item_list and not threw the database
        public List<Invoice> Invoice_Search_By_Date(DateTime start, DateTime end, List<Invoice> invoice_list)
        => invoice_list.FindAll(invoice => (start <= invoice.Date && invoice.Date <= end));

        // ILogic's Interface Functions
        public async Task Add_New_Element_To_Database(Invoice element)
        {
            if (element == null)
            {
                Service_Manager.Instance.Log_Service_Instance.Add_New_Exception_To_Log("Invoice entered can't be null!");
                throw new ArgumentNullException("Invoice entered can't be null!");
            }
            try
            {
                await _invoice_DB.Add_New_Element_To_DatabaseAsync(element);
            }
            catch (Invoice_Exception invoice_exception)
            {
                Service_Manager.Instance.Log_Service_Instance.Add_New_Exception_To_Log(invoice_exception.Message);
                throw invoice_exception;
            }
        }

        public async Task<List<Invoice>> Get_AllAsync()
        => (await _invoice_DB.Get_All()).OrderBy(invoice => invoice.Date).ToList();

        public async Task Remove_Element_From_Database(Invoice element)
        {
            if (element == null)
            {
                Service_Manager.Instance.Log_Service_Instance.Add_New_Exception_To_Log("Invoice entered can't be null!");
                throw new ArgumentNullException("Invoice entered can't be null!"); 
            }
            try
            {
                await _invoice_DB.Remove_Element_From_Database(element);
            }
            catch (Invoice_Exception invoice_exception)
            {
                Service_Manager.Instance.Log_Service_Instance.Add_New_Exception_To_Log(invoice_exception.Message);
                throw invoice_exception;
            }
        }

        public async Task Update_Database(List<Invoice> elements_list)
        {
            if (elements_list == null)
            {
                Service_Manager.Instance.Log_Service_Instance.Add_New_Exception_To_Log("Invoice list entered can't be null!");
                throw new ArgumentNullException("Invoice list entered can't be null!");
            }
            await _invoice_DB.Update_DatabaseAsync(elements_list);
        }
    }
}

