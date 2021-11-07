using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic_BLL
{
    public class Service_Manager // Holds all the services!
    {
        static Service_Manager _service__manager_instance;
        public Item_Service Item_Service_Instance;
        public User_Service User_Service_Instance;
        public Invoice_Service Invoice_Service_Instance;
        public Customer_Service Customer_Service_Instance;
        public Genre_Service Genre_Service_Instance;
        public Log_Service Log_Service_Instance;

        private Service_Manager()
        {
            Item_Service_Instance = new Item_Service();
            User_Service_Instance = new User_Service();
            Invoice_Service_Instance = new Invoice_Service();
            Customer_Service_Instance = new Customer_Service();
            Genre_Service_Instance = new Genre_Service();
            Log_Service_Instance = new Log_Service();
        }

        public static Service_Manager Instance //Singelton!
        {
            get
            {
                    if (_service__manager_instance == null)
                    {
                    _service__manager_instance = new Service_Manager();
                    }
                    return _service__manager_instance;
            }
        }

    }
}
