using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class Journal : Item
    {
        public int _issue;
        public Journal(int iSBN, string name, ItemType item_type, List<Genre> genre, DateTime date_printed, int amount_in_stock, double price, double discount, string author, int issue) : base(iSBN, item_type, name, genre, date_printed, amount_in_stock, price, discount, author)
        => _issue = issue;
        public override string ToString()
        => $"{Item_Type}: {Name}\nWritten by: {Author} on {Date_Printed:f}\nISBN: {ISBN} Issue: {_issue}\nGenres:\n{string.Join("\n", Genre_List)}\nPrice: {Price * (1 - Discount):c}   Stock: {Amount_In_Stock}\n";

    }
}
