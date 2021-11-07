using System;

namespace Common
{
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    public class Customer
    {
        public string First_name { get; private set; }
        public string Last_name { get; private set; }
        public readonly int ID;
        public readonly DateTime _birthday;
        double _discount;
        public double Discount { get => _discount; set { if (value < 1 && value >= 0) _discount = value; } }

        public Customer(string first_name, string last_name, int iD, DateTime birth_day, double discount)
        {
            First_name = first_name;
            Last_name = last_name;
            ID = iD;
            _birthday = birth_day;
            Discount = discount;
        }
        public Customer UpdateCustomer(string first_name, string last_name, double discount)
        {
            First_name = first_name;
            Last_name = last_name;
            Discount = discount;
            return this;
        }
        public override string ToString()
        => $"Name: {Last_name} {First_name}\nID: {ID}\nBirthDay: {_birthday:d}\nCurrent discount: {Discount*100}";

        public override bool Equals(object obj)
        {
            Customer other = obj as Customer;
            return ID == other.ID;
        }

    }
}
