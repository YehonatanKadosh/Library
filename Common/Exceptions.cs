using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    internal class Exceptions{} // only for the .cs fileName
    public class Genre_Exception: Exception 
    {
        public Genre_Exception(string message) : base(message) { }
    }

    public class User_Exception : Exception
    {
        public User_Exception(string message) : base(message) { }
    }
    public class Invoice_Exception : Exception
    {
        public Invoice_Exception(string message) : base(message) { }
    }
    public class Item_Exception : Exception
    {
        public Item_Exception(string message) : base(message) { }
    }
    public class Customer_Exception : Exception
    {
        public Customer_Exception(string message) : base(message) { }
    }
}
