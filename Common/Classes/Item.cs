using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    public abstract class Item
    {
        public readonly int ISBN;
        public ItemType Item_Type { get; private set; }
        public string Name { get; private set; }
        public List<Genre> Genre_List { get; private set; }
        public DateTime Date_Printed { get; private set; }
        public int Amount_In_Stock { get; protected set; }
        public double Price { get; set; }
        double _discount;
        public double Discount { get => _discount; set { if (value >= 0 && value < 1) _discount = value; } }
        public string Author { get; protected set; }


        protected Item(int iSBN, ItemType item_type, string name, List<Genre> genre, DateTime date_printed, int amount_in_stock, double price, double discount, string author)
        {
            ISBN = iSBN;
            Item_Type = item_type;
            Name = name;
            Genre_List = genre;
            Date_Printed = date_printed;
            Amount_In_Stock = amount_in_stock;
            Price = price;
            Discount = discount;
            Author = author;
        }

        public override bool Equals(object obj)
        {
            Item other = obj as Item;
            return ISBN == other.ISBN;
        }

        public void Restock(int amount)
        {
            Amount_In_Stock += amount;
        }
    }
}