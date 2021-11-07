using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class Book : Item
    {
        public string _summary;

        public Book(int iSBN, string name, ItemType item_type, List<Genre> genre, DateTime date_printed, int amount_in_stock, double price, double discount, string author, string summary) : base(iSBN, item_type, name, genre, date_printed, amount_in_stock, price, discount, author)
        => _summary = summary;

        public override string ToString()
        => $"{Item_Type}: {Name}\nWritten by: {Author} on {Date_Printed:f}\nISBN: {ISBN}\nGenres:\n{string.Join("\n", Genre_List)}\nSummary: {_summary}\nPrice: {Price * (1-Discount):c}   Stock: {Amount_In_Stock}\n";

    }
}
