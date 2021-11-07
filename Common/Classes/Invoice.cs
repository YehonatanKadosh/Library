using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    public class Invoice
    {
        public int Invoice_ID { get; private set; }
        public Customer Customer { get; private set; }
        public DateTime Date { get; private set; }
        public Item Item_For_Invoice { get; private set; }
        public double Price { get; private set; }

        public Invoice(int iNVOICE_ID, Customer customer, DateTime date, Item items, double price)
        {
            Invoice_ID = iNVOICE_ID;
            Customer = customer;
            Date = date;
            Item_For_Invoice = items;
            Price = price;
        }
       
        public override string ToString()
        => $"Invoice ID: {Invoice_ID} Date: {Date:d}\n\nCustomer details:\n{Customer}\n\nItem details:\n{Item_For_Invoice}\nPrice: {Price:c}";

        public override bool Equals(object obj)
        {
            Invoice other = obj as Invoice;
            return Invoice_ID == other.Invoice_ID;
        }
    }
}
