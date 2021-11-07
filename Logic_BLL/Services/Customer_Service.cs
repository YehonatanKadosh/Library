using Common;
using DataBase_DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Logic_BLL
{
    public class Customer_Service : ILogic<Customer>
    {
        readonly IData<Customer> _customer_db;
        public Customer_Service(IData<Customer> customerDb = null) // null for Release and Dummy for testing
        {
            if (customerDb == null)
                customerDb = new CustomerDB();
            _customer_db = customerDb;
        }

        /// <summary>
        /// Generate a new Customer
        /// </summary>
        /// <param name="first_name">Customer's first name</param>
        /// <param name="last_name">Customer's lasst name</param>
        /// <param name="ID">Customer's ID</param>
        /// <param name="birthday">Customer's Date of birth</param>
        /// <param name="discount">special customers discount - default is 10%</param>
        /// <returns>an instance of the new customer</returns>
        public async Task<Customer> Generate_CustomerAsync(string first_name, string last_name, int ID, DateTime birthday, double discount = 0.1)
        {
            if (first_name == null || last_name == null)
            {
                Service_Manager.Instance.Log_Service_Instance.Add_New_Exception_To_Log("Creating an instance of Customer with null as first name or last name is'nt possible!");
                throw new ArgumentNullException("Creating an instance of Customer with null as first name or last name is'nt possible!");
            }
            Customer new_customer = new Customer(first_name, last_name, ID, birthday, discount);
            await Add_New_Element_To_Database(new_customer);
            return new_customer;
        }

        /// <summary>
        /// Search customer by partial name
        /// </summary>
        /// <param name="partial_name">Customer's partial name</param>
        /// <returns>list of all matching customers by partial name</returns>
        public async Task<List<Customer>> Customer_Search_Partial_NameAsync(string partial_name)
        {
            if (partial_name == null)
            {
                Service_Manager.Instance.Log_Service_Instance.Add_New_Exception_To_Log("searching for null as partial name is'nt possible!");
                throw new ArgumentNullException("searching for null as partial name is'nt possible!");
            }
            return (await Get_AllAsync()).FindAll(customer => customer.First_name.ToLower().Contains(partial_name.ToLower()));
        }

        /// <summary>
        /// Search Customers by partial ID
        /// </summary>
        /// <param name="partial_id">partial id</param>
        /// <returns>list of all matching customers to the partial id</returns>
        public async Task<List<Customer>> Customer_Search_Partial_IDAsync(int partial_id)
        => (await Get_AllAsync()).FindAll(customer => customer.ID.ToString().Contains(partial_id.ToString()));

        /// <summary>
        /// Update Customer
        /// </summary>
        /// <param name="old">old cutomer</param>
        /// <param name="first_name">new first name</param>
        /// <param name="last_name">new last name</param>
        /// <param name="discount">new discount</param>
        /// <returns>instance of the new customer</returns>
        public async Task<Customer> Update_CustomerAsync(Customer old, string first_name, string last_name, double discount = 0.1)
        {
            if (first_name == null || last_name == null || old == null)
            {
                Service_Manager.Instance.Log_Service_Instance.Add_New_Exception_To_Log("Updating Customer with null as name or old customer is'nt possible!");
                throw new ArgumentNullException("Updating Customer with null as name or old customer is'nt possible!");
            }

            List<Customer> customer_list = await Get_AllAsync();
            Customer wanted_customer = customer_list.Find(c => c.ID == old.ID);
            wanted_customer.UpdateCustomer(first_name, last_name, discount);
            await Update_Database(customer_list);
            return wanted_customer;
        }

        // ILogic's Interface Functions
        public async Task Add_New_Element_To_Database(Customer element)
        {
            if (element == null)
            {
                Service_Manager.Instance.Log_Service_Instance.Add_New_Exception_To_Log("Customer entered can't be null!");
                throw new ArgumentNullException("Customer entered can't be null!");
            }
            try
            {
                await _customer_db.Add_New_Element_To_DatabaseAsync(element);
            }
            catch (Customer_Exception customer_exception)
            {
                Service_Manager.Instance.Log_Service_Instance.Add_New_Exception_To_Log(customer_exception.Message);
                throw customer_exception;
            }
        }

        public async Task<List<Customer>> Get_AllAsync()
        => (await _customer_db.Get_All()).OrderBy(customer => customer.First_name).ToList();

        public async Task Remove_Element_From_Database(Customer element)
        {
            if (element == null)
            {
                Service_Manager.Instance.Log_Service_Instance.Add_New_Exception_To_Log("Customer entered can't be null!");
                throw new ArgumentNullException("Customer entered can't be null!");
            }
            try
            {
                await _customer_db.Remove_Element_From_Database(element);
            }
            catch (Customer_Exception customer_exception)
            {
                Service_Manager.Instance.Log_Service_Instance.Add_New_Exception_To_Log(customer_exception.Message);
                throw customer_exception;
            }
        }

        public async Task Update_Database(List<Customer> elements_list)
        {
            if (elements_list == null)
            {
                Service_Manager.Instance.Log_Service_Instance.Add_New_Exception_To_Log("Customer list entered can't be null!");
                throw new ArgumentNullException("Customer list entered can't be null!");
            }
            await _customer_db.Update_DatabaseAsync(elements_list);
        }
    }
}
